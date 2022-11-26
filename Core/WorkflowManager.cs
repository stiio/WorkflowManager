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

public sealed class WorkflowManager<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    private readonly IWorkflowStepStore<TWorkflowStep> workflowStepStore;
    private readonly IServiceProvider services;
    private readonly StepsMetadata<TWorkflow, TWorkflowStep> stepsMetadata;
    private readonly IDictionary<StepKey, BaseStep<TWorkflow, TWorkflowStep>> allSteps;
    private readonly IDictionary<StepKey, BaseStep<TWorkflow, TWorkflowStep>> activeSteps;
    private readonly List<BaseStep<TWorkflow, TWorkflowStep>> sortedSteps;

    internal WorkflowManager(
        TWorkflow workflow,
        List<TWorkflowStep> workflowSteps,
        IServiceProvider services)
    {
        this.workflowStepStore = services.GetRequiredService<IWorkflowStepStore<TWorkflowStep>>();
        this.services = services;
        this.stepsMetadata = StepsMetadata<TWorkflow, TWorkflowStep>.GetInstance(WorkflowManagerOptions.TargetAssembly);
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

    public Guid WorkflowId => this.Workflow.Id;

    public TWorkflow Workflow { get; private set; }

    public List<TWorkflowStep> WorkflowSteps { get; private set; }

    public async Task<StepKey> Start<TStep>(string? relatedObjectId = null, object? payload = null)
        where TStep : BaseStep<TWorkflow, TWorkflowStep>
    {
        if (this.HasSteps())
        {
            throw new WorkflowManagerAlreadyHaveStepsException();
        }

        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        await this.CreateNextStep(stepKey, null, payload);

        return await this.Next();
    }

    public async Task<StepKey> Start<TStep, TData>(TData data, string? relatedObjectId = null, object? payload = null)
        where TStep : BaseStep<TWorkflow, TWorkflowStep, TData>
        where TData : class
    {
        if (this.HasSteps())
        {
            throw new WorkflowManagerAlreadyHaveStepsException();
        }

        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        await this.CreateNextStep(stepKey, null, payload);

        return await this.Next(data);
    }

    public async Task<StepKey> Next()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        var step = this.GetLastStep();

        if (this.stepsMetadata.GetStepMeta(step.StepKey.Step).HasData)
        {
            throw new WorkflowManagerStepRequireDataException(step.StepKey);
        }

        await this.workflowStepStore.Update(step.UpdateWorkflowStep());

        return await this.PrivateNext(step);
    }

    public async Task<StepKey> Next<TData>(TData data)
        where TData : class
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        var step = this.GetLastStep();

        if (step is not BaseStep<TWorkflow, TWorkflowStep, TData> typedStep)
        {
            throw new WorkflowManagerException($"{step.GetType().Name} not implement {typeof(BaseStep<TWorkflow, TWorkflowStep, TData>).Name}");
        }

        typedStep.Data = data;

        await this.workflowStepStore.Update(step.UpdateWorkflowStep());

        return await this.PrivateNext(step);
    }

    public Task<object> GetStepData()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        return this.GetLastStep().GetStepData();
    }

    public async Task<TStepData> GetStepData<TStepData>()
        where TStepData : class
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        return (TStepData)(await this.GetLastStep().GetStepData());
    }

    public async Task<StepKey> GoToPreviousStep()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        var step = this.GetLastStep();

        if (step.PreviousStepKey is null)
        {
            throw new WorkflowManagerPreviousStepException();
        }

        var previousStep = this.activeSteps[step.PreviousStepKey];

        await this.DeleteStepsAfter(previousStep);

        return previousStep.StepKey;
    }

    public Task<StepKey> GoToStep<TStep>(string? relatedObjectId = null)
        where TStep : BaseStep<TWorkflow, TWorkflowStep>
    {
        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        return this.GoToStep(stepKey);
    }

    public async Task<StepKey> GoToStep(StepKey stepKey)
    {
        var step = this.GetStep<BaseStep<TWorkflow, TWorkflowStep>>(stepKey);
        if (step is null)
        {
            throw new WorkflowManagerStepNotFoundException(stepKey);
        }

        await this.DeleteStepsAfter(step);

        return step.StepKey;
    }

    public TStep? GetStep<TStep>(string? relatedObjectId = null)
        where TStep : BaseStep<TWorkflow, TWorkflowStep>
    {
        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        return this.GetStep<TStep>(stepKey);
    }

    public TStep? GetStep<TStep>(StepKey stepKey)
        where TStep : BaseStep<TWorkflow, TWorkflowStep>
    {
        if (this.activeSteps.TryGetValue(stepKey, out var step))
        {
            return (TStep)step;
        }

        return null;
    }

    public BaseStep<TWorkflow, TWorkflowStep> GetLastStep()
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }

        return this.sortedSteps.Last();
    }

    public bool IsLastStep<TStep>(string? relatedObjectId = null)
        where TStep : BaseStep<TWorkflow, TWorkflowStep>
    {
        if (!this.HasSteps())
        {
            throw new WorkflowManagerNoStepsException();
        }
        
        var targetStepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        return this.IsLastStep(targetStepKey);
    }

    public bool IsLastStep(StepKey stepKey)
    {
        var lastStep = this.GetLastStep();

        return lastStep.StepKey == stepKey;
    }

    public bool HasStep(StepKey stepKey)
    {
        return this.activeSteps.ContainsKey(stepKey);
    }

    public bool HasStep<TStep>(string? relatedObjectId = null)
    {
        var stepKey = typeof(TStep).CreateStepKey(relatedObjectId);

        return this.HasStep(stepKey);
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

    public TCustomLogic? GetCustomLogicBefore<TCustomLogic>(StepKey beforeStepKey)
    {
        return this.sortedSteps
            .TakeWhile(step => step.StepKey != beforeStepKey)
            .OfType<TCustomLogic>()
            .LastOrDefault();
    }

    public IEnumerable<TCustomLogic> ListCustomLogic<TCustomLogic>()
    {
        return this.sortedSteps.OfType<TCustomLogic>();
    }

    private async Task<StepKey> PrivateNext(BaseStep<TWorkflow, TWorkflowStep> step)
    {
        if (step is not INextStep nextStep)
        {
            throw new WorkflowManagerNotImplementNextStepException(step.GetType());
        }

        var nextStepResult = await nextStep.Next();

        await this.CreateNextStep(nextStepResult.StepKey, step.StepKey, nextStepResult.Payload);

        return nextStepResult.StepKey;
    }

    private async Task CreateNextStep(StepKey stepKey, StepKey? previousStepKey, object? payload)
    {
        if (this.activeSteps.Keys.Any(key => key == stepKey))
        {
            throw new WorkflowManagerStepAlreadyExistsException(stepKey);
        }

        var meta = this.stepsMetadata.GetStepMeta(stepKey.Step);

        BaseStep<TWorkflow, TWorkflowStep> step;
        if (this.allSteps.TryGetValue(stepKey, out var existsStep))
        {
            existsStep.WorkflowStep.IsSoftDelete = false;
            existsStep.StepKey = stepKey;
            existsStep.PreviousStepKey = previousStepKey;

            if (meta.HasPayload)
            {
                meta.SetPayload(existsStep, payload);
            }

            await this.workflowStepStore.Update(existsStep.UpdateWorkflowStep());

            step = existsStep;
        }
        else
        {
            var newStep = meta.CreateStep<TWorkflow, TWorkflowStep>(this.services);

            var workflowStep = Activator.CreateInstance<TWorkflowStep>();
            workflowStep.WorkflowId = this.Workflow.Id;

            BaseStep<TWorkflow, TWorkflowStep>.InitStep(newStep, this, workflowStep, stepKey, previousStepKey);

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

    private async Task DeleteStepsAfter(BaseStep<TWorkflow, TWorkflowStep> targetStep)
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

    private BaseStep<TWorkflow, TWorkflowStep> CreateBaseStep(TWorkflowStep workflowStep)
    {
        var stepKey = JsonSerializer.Deserialize<StepKey>(workflowStep.StepKey!)!;
        var previousStepKey = JsonSerializer.Deserialize<StepKey>(workflowStep.PreviousStepKey ?? "null");
        var meta = this.stepsMetadata.GetStepMeta(stepKey.Step);

        var step = meta.CreateStep<TWorkflow, TWorkflowStep>(this.services);

        BaseStep<TWorkflow, TWorkflowStep>.InitStep(step, this, workflowStep, stepKey, previousStepKey);

        if (meta.HasData)
        {
            var data = JsonSerializer.Deserialize(workflowStep.Data ?? "null", meta.DataType!);
            meta.SetData(step, data);
        }

        if (meta.HasPayload)
        {
            var payload = JsonSerializer.Deserialize(workflowStep.Payload ?? "null", meta.PayloadType!);
            meta.SetPayload(step, payload);
        }

        return step;
    }
}