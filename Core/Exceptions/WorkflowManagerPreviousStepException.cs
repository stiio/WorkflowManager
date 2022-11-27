namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerPreviousStepException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerPreviousStepException"/> class.
    /// </summary>
    public WorkflowManagerPreviousStepException()
        : base("There are no steps left behind")
    {
    }
}