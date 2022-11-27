namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerNoStepsException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerNoStepsException"/> class.
    /// </summary>
    public WorkflowManagerNoStepsException()
        : base("Workflow no steps.")
    {
    }
}