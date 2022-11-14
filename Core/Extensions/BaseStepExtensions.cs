using Stio.WorkflowManager.Core.Models;

namespace Stio.WorkflowManager.Core.Extensions;

internal static class BaseStepExtensions
{
    public static IEnumerable<BaseStep> OrderByPreviousStep(this IEnumerable<BaseStep> steps)
    {
        var nextKey = StepKey.Create("Empty");
        var dictionary = steps.ToDictionary(step => step.PreviousStepKey ?? StepKey.Create("Empty"));
        
        while (dictionary.TryGetValue(nextKey, out var nextStep))
        {
            yield return nextStep;
        }
    }
}