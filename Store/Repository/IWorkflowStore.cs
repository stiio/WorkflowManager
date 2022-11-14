using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStore<TWorkflow>
    where TWorkflow : Workflow
{
    Task<TWorkflow?> FindById(Guid workflowId);
}

public interface IWorkflowStore : IWorkflowStore<Workflow>
{
}