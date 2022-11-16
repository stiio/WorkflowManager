using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

internal class WorkflowManagerBuilder : IWorkflowManagerBuilder
{
    private readonly IServiceCollection services;

    public WorkflowManagerBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore>() where TWorkflowStore : IWorkflowStore
    {
        services.TryAddScoped(typeof(IWorkflowStore), typeof(TWorkflowStore));

        return this;
    }

    public IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore>() where TWorkflowStepStore : IWorkflowStepStore
    {
        services.TryAddScoped(typeof(IWorkflowStepStore), typeof(TWorkflowStepStore));

        return this;
    }
}