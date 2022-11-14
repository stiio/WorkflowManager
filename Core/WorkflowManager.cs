using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Exceptions;
using Stio.WorkflowManager.Core.Extensions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.Core.Options;
using Stio.WorkflowManager.Store.Entity;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

public class WorkflowManager
{
    private readonly IWorkflowStepStore workflowStepStore;
    private readonly IServiceProvider services;
    private readonly StepsMetadata stepsMetadata;
    private readonly IDictionary<StepKey, BaseStep> allSteps;
    private readonly IDictionary<StepKey, BaseStep> activeSteps;
    private readonly List<BaseStep> sortedSteps;

    internal WorkflowManager(
        Workflow workflow,
        List<WorkflowStep> workflowSteps,
        IServiceProvider services)
    {
        this.workflowStepStore = services.GetRequiredService<IWorkflowStepStore>();
        this.services = services;
        this.stepsMetadata = StepsMetadata.GetInstance(WorkflowManagerOptions.TargetAssembly);
        this.Workflow = workflow;
        this.WorkflowSteps = workflowSteps.ToList();
        this.allSteps = workflowSteps.ToDictionary(
            step => JsonSerializer.Deserialize<StepKey>(step.StepKey!)!,
            this.CreateBaseStep);
        this.activeSteps = this.allSteps
            .Where(x => !x.Value.WorkflowStep.IsSoftDelete)
            .ToDictionary(x => x.Key, x => x.Value);
        this.sortedSteps = this.activeSteps.Values
            .Where(step => !step.WorkflowStep.IsSoftDelete)
            .OrderByPreviousStep()
            .ToList();
    }

    public Workflow Workflow { get; private set; }

    public List<WorkflowStep> WorkflowSteps { get; private set; }

    public Task<StepKey?> Start<TStep>(string? relatedObjectId = null, object? payload = null)
        where TStep : BaseStep
    {
        return this.PrivateStart<TStep>(relatedObjectId, payload);
    }

    public Task<StepKey?> Start<TStep, TData>(TData data, string? relatedObjectId = null, object? payload = null)
        where TStep : BaseStep
        where TData : class
    {
        return this.PrivateStart<TStep, TData>(data, relatedObjectId, payload);
    }

    public async Task<StepKey?> Next()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        var step = this.GetLastStep();

        step.WorkflowStep = await this.workflowStepStore.Update(step.UpdateWorkflowStep());

        return await this.PrivateNext(step);
    }

    public async Task<StepKey?> Next<TData>(TData data)
        where TData : class
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        var step = this.GetLastStep();

        if (step is not BaseStep<TData> typedStep)
        {
            throw new WorkflowManagerException($"{step.GetType().Name} not implement {nameof(BaseStep<TData>)}");
        }

        typedStep.Data = data;

        typedStep.WorkflowStep = await this.workflowStepStore.Update(step.UpdateWorkflowStep());

        return await this.PrivateNext(step);
    }

    public object? GetStepData()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        return this.GetLastStep().GetStepData();
    }

    public async Task<StepKey> GoToPreviousStep()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        var step = this.GetLastStep();

        if (step.PreviousStepKey is null)
        {
            throw new WorkflowManagerException("PreviousStepKey is null");
        }

        var previousStep = this.activeSteps[step.PreviousStepKey];

        await this.DeleteStepsAfter(previousStep);

        return previousStep.StepKey;
    }

    public async Task<StepKey> GoToStep<TStep>()
        where TStep : BaseStep
    {
        var step = this.GetStep<TStep>();

        if (step is null)
        {
            throw new WorkflowManagerException($"Step {typeof(TStep).Name} not found");
        }

        if (step.PreviousStepKey is null)
        {
            throw new WorkflowManagerException("PreviousStepKey is null");
        }

        var previousStep = this.activeSteps[step.PreviousStepKey];

        await this.DeleteStepsAfter(previousStep);

        return previousStep.StepKey;
    }

    public TStep? GetStep<TStep>(string? relatedObjectId = null)
        where TStep : BaseStep
    {
        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        if (this.activeSteps.TryGetValue(stepKey, out var step))
        {
            return step as TStep;
        }

        return null;
    }

    public BaseStep GetLastStep()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        return this.sortedSteps.Last();
    }

    public bool IsLastStep<TStep>(string? relatedObjectId)
        where TStep : BaseStep
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow no steps");
        }

        var lastStep = this.GetLastStep();

        var targetStepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        return lastStep.StepKey == targetStepKey;
    }

    public bool HasSteps()
    {
        return this.sortedSteps.Any();
    }

    public TCustomLogic? GetFirstCustomLogic<TCustomLogic>()
    {
        return this.sortedSteps.OfType<TCustomLogic>().FirstOrDefault();
    }

    public TCustomLogic? GetLastCustomLogic<TCustomLogic>()
    {
        return this.sortedSteps.OfType<TCustomLogic>().LastOrDefault();
    }

    public TCustomLogic? GetCustomLogicBefore<TCustomLogic>(BaseStep beforeStep)
    {
        return this.sortedSteps
            .TakeWhile(step => step != beforeStep)
            .OfType<TCustomLogic>()
            .LastOrDefault();
    }

    public IEnumerable<TCustomLogic> ListCustomLogic<TCustomLogic>()
    {
        return this.sortedSteps.OfType<TCustomLogic>();
    }

    private async Task DeleteStepsAfter(BaseStep targetStep)
    {
        var deletedSteps = this.sortedSteps
            .SkipWhile(step => step != targetStep)
            .Skip(1)
            .ToArray();

        var workflowSteps = deletedSteps.Select(step => step.WorkflowStep).ToArray();
        foreach (var workflowStep in workflowSteps)
        {
            workflowStep.IsSoftDelete = true;
        }

        await this.workflowStepStore.UpdateRange(workflowSteps);

        foreach (var deletedStep in deletedSteps)
        {
            this.activeSteps.Remove(deletedStep.StepKey);
        }

        this.sortedSteps.RemoveAll(step => deletedSteps.Contains(step));
    }

    private async Task<StepKey?> PrivateNext(BaseStep step)
    {
        if (step is not INextStep nextStep)
        {
            throw new WorkflowManagerException($"{step.GetType().Name} not implement {nameof(INextStep)}");
        }

        var nextStepResult = await nextStep.Next();

        if (nextStepResult.StepKey is not null)
        {
            await this.CreateNextStep(nextStepResult.StepKey, step.PreviousStepKey, nextStepResult.Payload);
        }

        return nextStepResult.StepKey;
    }

    private async Task<StepKey?> PrivateStart<TStep, TData>(TData data, string? relatedObjectId, object? payload)
        where TStep : BaseStep
        where TData : class
    {
        if (this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow already have steps");
        }

        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        await this.CreateNextStep(stepKey, null, payload);

        return await this.Next(data);
    }

    private async Task<StepKey?> PrivateStart<TStep>(string? relatedObjectId, object? payload)
        where TStep : BaseStep
    {
        if (this.HasSteps())
        {
            throw new WorkflowManagerException("Workflow already have steps");
        }

        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        await this.CreateNextStep(stepKey, null, payload);

        return await this.Next();
    }

    private async Task CreateNextStep(StepKey stepKey, StepKey? previousStepKey, object? payload = null)
    {
        if (this.activeSteps.Keys.Any(key => key == stepKey))
        {
            throw new WorkflowManagerException($"Step {stepKey.Step}:{stepKey.RelatedObjectId} already exists");
        }

        var meta = this.stepsMetadata.GetStepMeta(stepKey.Step);

        BaseStep step;
        if (this.allSteps.TryGetValue(stepKey, out var existsStep))
        {
            existsStep.WorkflowStep.IsSoftDelete = false;
            existsStep.StepKey = stepKey;
            existsStep.PreviousStepKey = previousStepKey;

            if (meta.HasPayload)
            {
                meta.SetPayload(existsStep, payload);
            }

            existsStep.WorkflowStep = await this.workflowStepStore.Update(existsStep.UpdateWorkflowStep());

            step = existsStep;
        }
        else
        {
            var newStep = meta.CreateStep(this.services);

            var workflowStep = new WorkflowStep()
            {
                WorkflowId = this.Workflow.Id,
            };

            BaseStep.InitStep(newStep, this, workflowStep, stepKey, previousStepKey);

            if (meta.HasPayload)
            {
                meta.SetPayload(newStep, payload);
            }

            newStep.WorkflowStep = await this.workflowStepStore.Create(newStep.UpdateWorkflowStep());
            this.WorkflowSteps.Add(newStep.WorkflowStep);

            step = newStep;
        }

        this.allSteps.TryAdd(step.StepKey, step);
        this.activeSteps.TryAdd(step.StepKey, step);
        this.sortedSteps.Add(step);
    }

    private BaseStep CreateBaseStep(WorkflowStep workflowStep)
    {
        var stepKey = JsonSerializer.Deserialize<StepKey>(workflowStep.StepKey!)!;
        var previousStepKey = JsonSerializer.Deserialize<StepKey>(workflowStep.PreviousStepKey ?? "null");
        var meta = this.stepsMetadata.GetStepMeta(stepKey.Step);

        var step = meta.CreateStep(this.services);

        BaseStep.InitStep(step, this, workflowStep, stepKey, previousStepKey);

        if (meta.HasData)
        {
            meta.SetData(step, JsonSerializer.Deserialize(workflowStep.Data ?? "null", meta.DataType!));
        }

        if (meta.HasPayload)
        {
            meta.SetPayload(step, JsonSerializer.Deserialize(workflowStep.Payload ?? "null", meta.PayloadType!));
        }

        return step;
    }
}