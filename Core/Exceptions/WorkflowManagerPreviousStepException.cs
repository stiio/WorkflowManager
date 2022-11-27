namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerPreviousStepException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerPreviousStepException() : base("There are no steps left behind")
    {
    }
}