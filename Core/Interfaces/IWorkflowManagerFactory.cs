namespace Stio.WorkflowManager.Core.Interfaces;

public interface IWorkflowManagerFactory
{
    Task<WorkflowManager> CreateWorkflowManager(Guid workflowId);
}