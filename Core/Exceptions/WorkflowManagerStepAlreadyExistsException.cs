using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepAlreadyExistsException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerStepAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="stepKey">StepKey</param>
    public WorkflowManagerStepAlreadyExistsException(StepKey stepKey)
        : base(!string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step}:{stepKey.RelatedObjectId} already exists"
            : $"Step {stepKey.Step} already exists")
    {
    }
}