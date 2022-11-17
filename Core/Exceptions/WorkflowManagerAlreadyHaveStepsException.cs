namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerAlreadyHaveStepsException : WorkflowManagerException
{
    public WorkflowManagerAlreadyHaveStepsException() : base("Workflow already have steps. Use NextStep for continue.")
    {
    }
}