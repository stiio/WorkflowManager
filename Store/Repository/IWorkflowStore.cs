using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStore
{
    Task<IWorkflow?> FindById(Guid workflowId);
}