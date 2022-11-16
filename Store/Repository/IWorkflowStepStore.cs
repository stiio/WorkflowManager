using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStepStore<TWorkflowStep> where TWorkflowStep : class, IWorkflowStep
{
    Task<TWorkflowStep> Create(TWorkflowStep workflowStep);

    Task<TWorkflowStep> Update(TWorkflowStep workflowStep);

    Task<TWorkflowStep[]> UpdateRange(TWorkflowStep[] workflowSteps);

    Task<TWorkflowStep[]> ListStepsByWorkflowId(Guid workflowId);
}