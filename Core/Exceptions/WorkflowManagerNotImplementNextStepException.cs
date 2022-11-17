using Stio.WorkflowManager.Core.Interfaces;

namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerNotImplementNextStepException : WorkflowManagerException
{
    public WorkflowManagerNotImplementNextStepException(Type stepType) : base($"{stepType.Name} not implement {nameof(INextStep)}")
    {
    }
}