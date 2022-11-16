using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Options;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core;

public static class ConfigureServices
{
    public static IWorkflowManagerBuilder AddWorkflowManager<TWorkflow, TWorkflowStep>(this IServiceCollection services, Type stepsAssemblyMarkerType)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        WorkflowManagerOptions.TargetAssembly = stepsAssemblyMarkerType.Assembly;

        services.AddScoped<IWorkflowManagerFactory<TWorkflow, TWorkflowStep>, WorkflowManagerFactory<TWorkflow, TWorkflowStep>>();

        return new WorkflowManagerBuilder(typeof(TWorkflow), typeof(TWorkflowStep), services);
    }
}