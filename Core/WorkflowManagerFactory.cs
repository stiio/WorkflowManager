using Stio.WorkflowManager.Core.Exceptions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

internal class WorkflowManagerFactory : IWorkflowManagerFactory
{
    private readonly IWorkflowStore workflowStore;
    private readonly IWorkflowStepStore workflowStepStore;
    private readonly IServiceProvider services;

    public WorkflowManagerFactory(
        IWorkflowStore workflowStore,
        IWorkflowStepStore workflowStepStore,
        IServiceProvider services)
    {
        this.workflowStore = workflowStore;
        this.workflowStepStore = workflowStepStore;
        this.services = services;
    }

    public async Task<WorkflowManager> CreateWorkflowManager(Guid workflowId)
    {
        var workflow = await this.workflowStore.FindById(workflowId);

        if (workflow == null)
        {
            throw new WorkflowManagerException($"Workflow {workflowId} not found");
        }

        var steps = await this.workflowStepStore.ListStepsByWorkflowId(workflowId);

        return new WorkflowManager(workflow, steps.ToList(), this.services);
    }
}