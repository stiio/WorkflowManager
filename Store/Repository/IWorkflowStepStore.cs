using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStepStore
{
    Task<IWorkflowStep> Create(IWorkflowStep workflowStep);

    Task<IWorkflowStep> Update(IWorkflowStep workflowStep);

    Task<IWorkflowStep[]> UpdateRange(IWorkflowStep[] workflowSteps);

    Task<IWorkflowStep[]> ListStepsByWorkflowId(Guid workflowId);
}