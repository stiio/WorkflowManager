using Stio.WorkflowManager.Store.Entity;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core.Interfaces;

public interface IWorkflowManagerBuilder
{
    IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore, TWorkflow>()
        where TWorkflowStore : IWorkflowStore<TWorkflow>
        where TWorkflow : IWorkflow;

    IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore, TWorkflowStep>()
        where TWorkflowStepStore : IWorkflowStepStore<TWorkflowStep>
        where TWorkflowStep : IWorkflowStep;
}