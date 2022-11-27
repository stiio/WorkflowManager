namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerAlreadyHaveStepsException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerAlreadyHaveStepsException() : base("Workflow already have steps. Use NextStep for continue.")
    {
    }
}