using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.DemoApi.WorkflowManager.Store;

public class WorkflowStore : IWorkflowStore<Workflow>
{
    private readonly ApplicationDbContext context;

    public WorkflowStore(ApplicationDbContext context)
    {
        this.context = context;
    }

    public Task<Workflow?> FindById(Guid workflowId)
    {
        return this.context.Workflows.FirstOrDefaultAsync(workflow => workflow.Id == workflowId);
    }
}