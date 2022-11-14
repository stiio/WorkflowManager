using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Options;

namespace Stio.WorkflowManager.Core;

public static class ConfigureServices
{
    public static void AddWorkflowManager(this IServiceCollection services, Type stepsAssemblyMarkerType)
    {
        WorkflowManagerOptions.TargetAssembly = stepsAssemblyMarkerType.Assembly;

        services.AddScoped<IWorkflowManagerFactory, WorkflowManagerFactory>();
    }
}