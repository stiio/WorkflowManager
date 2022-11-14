using System.Reflection;

namespace Stio.WorkflowManager.Core.Options;

internal static class WorkflowManagerOptions
{
    public static Assembly TargetAssembly { get; set; } = null!;
}