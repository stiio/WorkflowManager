using Stio.WorkflowManager.Core.Interfaces;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerNotImplementNextStepException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerNotImplementNextStepException(Type stepType) : base($"{stepType.Name} not implement {nameof(INextStep)}")
    {
    }
}