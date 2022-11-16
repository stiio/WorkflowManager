using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.Store.Entity;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.DemoApi.Services;

public class WorkflowStepStore : IWorkflowStepStore
{
    private readonly ApplicationDbContext context;

    public WorkflowStepStore(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IWorkflowStep> Create(IWorkflowStep workflowStep)
    {
        var step = new WorkflowStep()
        {
            Id = workflowStep.Id,
            WorkflowId = workflowStep.WorkflowId,
            Data = workflowStep.Data,
            Payload = workflowStep.Payload,
            StepKey = workflowStep.StepKey,
            PreviousStepKey = workflowStep.PreviousStepKey,
            IsSoftDelete = workflowStep.IsSoftDelete,
        };

        await this.context.WorkflowSteps.AddAsync(step);
        await this.context.SaveChangesAsync();

        return step;
    }

    public async Task<IWorkflowStep> Update(IWorkflowStep workflowStep)
    {
        var step = await this.context.WorkflowSteps.FirstAsync(x => x.Id == workflowStep.Id);

        step.Data = workflowStep.Data;
        step.Payload = workflowStep.Payload;
        step.StepKey = workflowStep.StepKey;
        step.PreviousStepKey = workflowStep.PreviousStepKey;
        step.IsSoftDelete = workflowStep.IsSoftDelete;

        this.context.WorkflowSteps.Update(step);
        await context.SaveChangesAsync();

        return step;
    }

    public async Task<IWorkflowStep[]> UpdateRange(IWorkflowStep[] workflowSteps)
    {
        var stepIds = workflowSteps.Select(x => x.Id).ToArray();

        var steps = await this.context.WorkflowSteps
            .Where(x => stepIds.Contains(x.Id))
            .ToArrayAsync();

        foreach (var step in steps)
        {
            var workflowStep = workflowSteps.First(x => x.Id == step.Id);

            step.Data = workflowStep.Data;
            step.Payload = workflowStep.Payload;
            step.StepKey = workflowStep.StepKey;
            step.PreviousStepKey = workflowStep.PreviousStepKey;
            step.IsSoftDelete = workflowStep.IsSoftDelete;
        }

        this.context.WorkflowSteps.UpdateRange(steps);
        await context.SaveChangesAsync();

        return steps as IWorkflowStep[];
    }

    public async Task<IWorkflowStep[]> ListStepsByWorkflowId(Guid workflowId)
    {
        return await this.context.WorkflowSteps
            .Where(workflowStep => workflowStep.WorkflowId == workflowId)
            .ToArrayAsync();
    }
}