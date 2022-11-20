using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

public class WorkflowManagerStepRequireDataException : WorkflowManagerException
{
    public WorkflowManagerStepRequireDataException(StepKey stepKey)
        : base($"Step {stepKey.Step} require data.")
    {
    }
}