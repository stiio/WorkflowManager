using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerStepNotFoundException : WorkflowManagerException
{
    public WorkflowManagerStepNotFoundException(StepKey stepKey)
        : base(string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step} not found"
            : $"Step {stepKey.Step}:{stepKey.RelatedObjectId} not found")
    {
    }
}