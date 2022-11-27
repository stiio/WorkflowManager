using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Interfaces;

/// <summary>
/// INextStep
/// </summary>
public interface INextStep
{
    /// <summary>
    /// Performs some kind of business logic and returns <see cref="NextStepResult"/>
    /// </summary>
    /// <returns></returns>
    Task<NextStepResult> Next();
}