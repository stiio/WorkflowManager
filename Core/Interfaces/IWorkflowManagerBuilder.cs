namespace Stio.WorkflowManager.Core.Interfaces;

/// <summary>
/// IWorkflowManagerBuilder
/// </summary>
public interface IWorkflowManagerBuilder
{
    /// <summary>
    /// Register implementation of IWorkflowStore
    /// </summary>
    /// <typeparam name="TWorkflowStore">Implementation of IWorkflowStore</typeparam>
    /// <returns></returns>
    IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore>()
        where TWorkflowStore : class;

    /// <summary>
    /// Register implementation of IWorkflowStepStore
    /// </summary>
    /// <typeparam name="TWorkflowStepStore">Implementation of IWorkflowStepStore</typeparam>
    /// <returns></returns>
    IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore>()
        where TWorkflowStepStore : class;
}