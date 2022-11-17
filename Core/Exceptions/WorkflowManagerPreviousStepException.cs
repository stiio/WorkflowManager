namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerPreviousStepException : WorkflowManagerException
{
    public WorkflowManagerPreviousStepException() : base("There are no steps left behind")
    {
    }
}