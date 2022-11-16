namespace Stio.WorkflowManager.Core.Interfaces;

public interface IWorkflowManagerBuilder
{
    IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore>()
        where TWorkflowStore : class;

    IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore>()
        where TWorkflowStepStore : class;
}