using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Interfaces;

public interface INextStep
{
    Task<NextStepResult> Next();
}