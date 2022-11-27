using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

internal class WorkflowManagerBuilder : IWorkflowManagerBuilder
{
    private readonly Type workflowType;
    private readonly Type workflowStepType;
    private readonly IServiceCollection services;

    public WorkflowManagerBuilder(
        Type workflowType,
        Type workflowStepType,
        IServiceCollection services)
    {
        this.workflowType = workflowType;
        this.workflowStepType = workflowStepType;
        this.services = services;
    }

    public IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore>()
        where TWorkflowStore : class
    {
        this.services.TryAddScoped(typeof(IWorkflowStore<>).MakeGenericType(this.workflowType), typeof(TWorkflowStore));

        return this;
    }

    public IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore>()
        where TWorkflowStepStore : class
    {
        this.services.TryAddScoped(typeof(IWorkflowStepStore<>).MakeGenericType(this.workflowStepType), typeof(TWorkflowStepStore));

        return this;
    }
}