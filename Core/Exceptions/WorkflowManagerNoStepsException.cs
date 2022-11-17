namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerNoStepsException : WorkflowManagerException
{
    public WorkflowManagerNoStepsException() : base("Workflow no steps.")
    {
    }
}