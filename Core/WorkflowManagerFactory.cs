using Stio.WorkflowManager.Core.Exceptions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Store.Entity;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

internal class WorkflowManagerFactory<TWorkflow, TWorkflowStep> : IWorkflowManagerFactory<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    private readonly IWorkflowStore<TWorkflow> workflowStore;
    private readonly IWorkflowStepStore<TWorkflowStep> workflowStepStore;
    private readonly IServiceProvider services;

    public WorkflowManagerFactory(
        IWorkflowStore<TWorkflow> workflowStore,
        IWorkflowStepStore<TWorkflowStep> workflowStepStore,
        IServiceProvider services)
    {
        this.workflowStore = workflowStore;
        this.workflowStepStore = workflowStepStore;
        this.services = services;
    }

    public async Task<WorkflowManager<TWorkflow, TWorkflowStep>> CreateWorkflowManager(Guid workflowId)
    {
        var workflow = await this.workflowStore.FindById(workflowId).ConfigureAwait(false);

        if (workflow == null)
        {
            throw new WorkflowManagerException($"Workflow {workflowId} not found");
        }

        var steps = await this.workflowStepStore.ListStepsByWorkflowId(workflowId).ConfigureAwait(false);

        return new WorkflowManager<TWorkflow, TWorkflowStep>(workflow, steps.ToList(), this.services);
    }
}