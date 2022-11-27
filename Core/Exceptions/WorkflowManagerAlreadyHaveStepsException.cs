namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerAlreadyHaveStepsException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerAlreadyHaveStepsException"/> class.
    /// </summary>
    public WorkflowManagerAlreadyHaveStepsException()
        : base("Workflow already have steps. Use NextStep for continue.")
    {
    }
}