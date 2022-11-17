using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerStepAlreadyExistsException : WorkflowManagerException
{
    public WorkflowManagerStepAlreadyExistsException(StepKey stepKey)
        : base(!string.IsNullOrEmpty(stepKey.RelatedObjectId)
            ? $"Step {stepKey.Step}:{stepKey.RelatedObjectId} already exists"
            : $"Step {stepKey.Step} already exists")
    {
    }
}