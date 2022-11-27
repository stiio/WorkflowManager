using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

/// <summary>
/// Workflow Store
/// </summary>
/// <typeparam name="TWorkflow"></typeparam>
public interface IWorkflowStore<TWorkflow> where TWorkflow : class, IWorkflow
{
    /// <summary>
    /// Find workflow by id
    /// </summary>
    /// <param name="workflowId">Id of workflow</param>
    /// <returns>Return the found workflow or null</returns>
    Task<TWorkflow?> FindById(Guid workflowId);
}