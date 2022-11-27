using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepRequireDataException : WorkflowManagerException
{
    /// <inheritdoc />
    public WorkflowManagerStepRequireDataException(StepKey stepKey)
        : base($"Step {stepKey.Step} require data.")
    {
    }
}