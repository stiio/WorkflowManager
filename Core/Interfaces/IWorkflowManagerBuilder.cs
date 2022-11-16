using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core.Interfaces;

public interface IWorkflowManagerBuilder
{
    IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore>()
        where TWorkflowStore : IWorkflowStore;

    IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore>()
        where TWorkflowStepStore : IWorkflowStepStore;
}