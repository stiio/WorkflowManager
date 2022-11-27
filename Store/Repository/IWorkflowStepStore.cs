using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

/// <summary>
/// Workflow Step Store
/// </summary>
/// <typeparam name="TWorkflowStep">Implementation of IWorkflowStep</typeparam>
public interface IWorkflowStepStore<TWorkflowStep> where TWorkflowStep : class, IWorkflowStep
{
    /// <summary>
    /// Create workflow step
    /// </summary>
    /// <param name="workflowStep">Step for created</param>
    /// <returns>Return created workflow step</returns>
    Task<TWorkflowStep> Create(TWorkflowStep workflowStep);

    /// <summary>
    /// Update workflow step
    /// </summary>
    /// <param name="workflowStep">Step for update</param>
    /// <returns></returns>
    Task Update(TWorkflowStep workflowStep);

    /// <summary>
    /// Update multiply workflow steps
    /// </summary>
    /// <param name="workflowSteps">Steps for updated</param>
    /// <returns></returns>
    Task UpdateRange(TWorkflowStep[] workflowSteps);

    /// <summary>
    /// List all steps by workflowId
    /// </summary>
    /// <param name="workflowId">Id of workflow</param>
    /// <returns></returns>
    Task<TWorkflowStep[]> ListStepsByWorkflowId(Guid workflowId);
}