using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Store.Repository;

public interface IWorkflowStepStore<TWorkflowStep> where TWorkflowStep : class, IWorkflowStep
{
    Task<TWorkflowStep> Create(TWorkflowStep workflowStep);

    Task Update(TWorkflowStep workflowStep);

    Task UpdateRange(TWorkflowStep[] workflowSteps);

    Task<TWorkflowStep[]> ListStepsByWorkflowId(Guid workflowId);
}