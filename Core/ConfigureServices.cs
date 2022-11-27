using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Options;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core;

/// <summary>
/// Configure Services Extensions
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Register <see cref="IWorkflowManagerFactory{TWorkflow,TWorkflowStep}"/>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="stepsAssemblyMarkerType"></param>
    /// <typeparam name="TWorkflow">Implementation of <see cref="IWorkflow"/></typeparam>
    /// <typeparam name="TWorkflowStep">Implementation of <see cref="IWorkflowStep"/></typeparam>
    /// <returns><see cref="IWorkflowManagerBuilder"/></returns>
    public static IWorkflowManagerBuilder AddWorkflowManager<TWorkflow, TWorkflowStep>(this IServiceCollection services, Type stepsAssemblyMarkerType)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        WorkflowManagerOptions.TargetAssembly = stepsAssemblyMarkerType.Assembly;

        services.AddScoped<IWorkflowManagerFactory<TWorkflow, TWorkflowStep>, WorkflowManagerFactory<TWorkflow, TWorkflowStep>>();

        return new WorkflowManagerBuilder(typeof(TWorkflow), typeof(TWorkflowStep), services);
    }
}