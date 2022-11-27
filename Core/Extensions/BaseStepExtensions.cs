using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Extensions;

internal static class BaseStepExtensions
{
    public static IEnumerable<BaseStep<TWorkflow, TWorkflowStep>> OrderByPreviousStep<TWorkflow, TWorkflowStep>(
        this IEnumerable<BaseStep<TWorkflow, TWorkflowStep>> steps)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        var nextKey = StepKey.Create("Empty");
        var dictionary = steps.ToDictionary(step => step.PreviousStepKey ?? StepKey.Create("Empty"));

        while (dictionary.TryGetValue(nextKey, out var nextStep))
        {
            nextKey = nextStep.StepKey;
            yield return nextStep;
        }
    }
}