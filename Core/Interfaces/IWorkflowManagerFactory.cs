using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Interfaces;

public interface IWorkflowManagerFactory<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    Task<WorkflowManager<TWorkflow, TWorkflowStep>> CreateWorkflowManager(Guid workflowId);
}