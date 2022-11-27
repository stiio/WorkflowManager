using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerStepRequireDataException : WorkflowManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerStepRequireDataException"/> class.
    /// </summary>
    /// <param name="stepKey">StepKey</param>
    public WorkflowManagerStepRequireDataException(StepKey stepKey)
        : base($"Step {stepKey.Step} require data.")
    {
    }
}