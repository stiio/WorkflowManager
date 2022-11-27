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

    public async Task Update(WorkflowStep workflowStep)
    {
        this.context.WorkflowSteps.Update(workflowStep);
        await this.context.SaveChangesAsync();
    }

    public async Task UpdateRange(WorkflowStep[] workflowSteps)
    {
        this.context.WorkflowSteps.UpdateRange(workflowSteps);
        await this.context.SaveChangesAsync();
    }

    public async Task<WorkflowStep[]> ListStepsByWorkflowId(Guid workflowId)
    {
        return await this.context.WorkflowSteps
            .Where(workflowStep => workflowStep.WorkflowId == workflowId)
            .ToArrayAsync();
    }
}