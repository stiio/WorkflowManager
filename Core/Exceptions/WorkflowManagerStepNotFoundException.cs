using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepNotFoundException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerStepNotFoundException"/> class.
    /// </summary>
    /// <param name="stepKey">StepKey</param>
    public WorkflowManagerStepNotFoundException(StepKey stepKey)
        : base(string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step} not found"
            : $"Step {stepKey.Step}:{stepKey.RelatedObjectId} not found")
    {
    }
}