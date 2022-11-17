namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerStepNotFoundException : WorkflowManagerException
{
    public WorkflowManagerStepNotFoundException(Type stepType) : base($"Step {stepType.Name} not found")
    {
    }
}