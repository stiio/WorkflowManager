using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepNotFoundException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerStepNotFoundException(StepKey stepKey)
        : base(string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step} not found"
            : $"Step {stepKey.Step}:{stepKey.RelatedObjectId} not found")
    {
    }
}