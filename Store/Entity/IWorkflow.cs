namespace Stio.WorkflowManager.Store.Entity;

/// <summary>
/// Workflow
/// </summary>
public interface IWorkflow
{
    /// <summary>
    /// Id of workflow
    /// </summary>
    public Guid Id { get; set; }
}