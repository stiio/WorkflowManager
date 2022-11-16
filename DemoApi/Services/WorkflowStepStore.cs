using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.DemoApi.Services;

public class WorkflowStepStore : IWorkflowStepStore<WorkflowStep>
{
    private readonly ApplicationDbContext context;

    public WorkflowStepStore(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<WorkflowStep> Create(WorkflowStep workflowStep)
    {
        await this.context.WorkflowSteps.AddAsync(workflowStep);
        await this.context.SaveChangesAsync();

        return workflowStep;
    }

    public async Task<WorkflowStep> Update(WorkflowStep workflowStep)
    {
        this.context.WorkflowSteps.Update(workflowStep);
        await context.SaveChangesAsync();

        return workflowStep;
    }

    public async Task<WorkflowStep[]> UpdateRange(WorkflowStep[] workflowSteps)
    {
        this.context.WorkflowSteps.UpdateRange(workflowSteps);
        await context.SaveChangesAsync();

        return workflowSteps;
    }

    public async Task<WorkflowStep[]> ListStepsByWorkflowId(Guid workflowId)
    {
        return await this.context.WorkflowSteps
            .Where(workflowStep => workflowStep.WorkflowId == workflowId)
            .ToArrayAsync();
    }
}