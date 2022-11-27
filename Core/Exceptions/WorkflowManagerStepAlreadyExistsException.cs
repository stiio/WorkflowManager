using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepAlreadyExistsException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerStepAlreadyExistsException(StepKey stepKey)
        : base(!string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step}:{stepKey.RelatedObjectId} already exists"
            : $"Step {stepKey.Step} already exists")
    {
    }
}