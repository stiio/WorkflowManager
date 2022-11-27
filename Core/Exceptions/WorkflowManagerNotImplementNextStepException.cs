using Stio.WorkflowManager.Core.Interfaces;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerNotImplementNextStepException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerNotImplementNextStepException"/> class.
    /// </summary>
    /// <param name="stepType">Type of step</param>
    public WorkflowManagerNotImplementNextStepException(Type stepType)
        : base($"{stepType.Name} not implement {nameof(INextStep)}")
    {
    }
}