using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Interfaces;

/// <summary>
/// IWorkflowManagerFactory
/// </summary>
/// <typeparam name="TWorkflow"></typeparam>
/// <typeparam name="TWorkflowStep"></typeparam>
public interface IWorkflowManagerFactory<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    /// <summary>
    /// Create workflow manager by workflowId
    /// </summary>
    /// <param name="workflowId">Id of workflow</param>
    /// <returns><see cref="WorkflowManager{TWorkflow,TWorkflowStep}"/></returns>
    Task<WorkflowManager<TWorkflow, TWorkflowStep>> CreateWorkflowManager(Guid workflowId);
}