namespace Stio.WorkflowManager.Store.Entity;

/// <summary>
/// Workflow Step
/// </summary>
public interface IWorkflowStep
{
    /// <summary>
    /// Id of workflow step
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of related workflow
    /// </summary>
    public Guid WorkflowId { get; set; }

    /// <summary>
    /// Submitted data of workflow step
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Payload of workflow step
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// Unique step key
    /// </summary>
    public string? StepKey { get; set; }

    /// <summary>
    /// Step key of previous step
    /// </summary>
    public string? PreviousStepKey { get; set; }

    /// <summary>
    /// Flag indicating whether this step has been deleted.
    /// </summary>
    public bool IsSoftDelete { get; set; }
}