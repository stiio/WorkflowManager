using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Options;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core;

public static class ConfigureServices
{
    public static IWorkflowManagerBuilder AddWorkflowManager(this IServiceCollection services, Type stepsAssemblyMarkerType)
    {
        WorkflowManagerOptions.TargetAssembly = stepsAssemblyMarkerType.Assembly;

        services.AddScoped<IWorkflowManagerFactory, WorkflowManagerFactory>();

        return new WorkflowManagerBuilder(services);
    }
}