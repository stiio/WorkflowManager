using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Store.Entity;
using Stio.WorkflowManager.Store.Repository;

namespace Stio.WorkflowManager.Core;

internal class WorkflowManagerBuilder : IWorkflowManagerBuilder
{
    private readonly IServiceCollection services;

    public WorkflowManagerBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public IWorkflowManagerBuilder AddWorkflowStore<TWorkflowStore, TWorkflow>()
        where TWorkflowStore : IWorkflowStore<TWorkflow>
        where TWorkflow : IWorkflow
    {
        services.TryAddScoped(typeof(IWorkflowStore<TWorkflow>), typeof(TWorkflowStore));

        return this;
    }

    public IWorkflowManagerBuilder AddWorkflowStepStore<TWorkflowStepStore, TWorkflowStep>() where TWorkflowStepStore : IWorkflowStepStore<TWorkflowStep> where TWorkflowStep : IWorkflowStep
    {
        services.TryAddScoped(typeof(IWorkflowStepStore<TWorkflowStep>), typeof(TWorkflowStepStore));

        return this;
    }
}