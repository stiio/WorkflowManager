namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerNoStepsException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerNoStepsException() : base("Workflow no steps.")
    {
    }
}