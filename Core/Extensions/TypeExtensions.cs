using System.Reflection;
using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Extensions;

internal static class TypeExtensions
{
    public static StepKey CreateStepKey(this Type type, string? relatedObjectId = null)
    {
        var targetStepAttribute = type.GetCustomAttribute<StepAttribute>()!;
        return StepKey.Create(targetStepAttribute.Step, relatedObjectId);
    }
}