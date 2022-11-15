using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStore<TWorkflow>
    where TWorkflow : IWorkflow
{
    Task<TWorkflow?> FindById(Guid workflowId);
}