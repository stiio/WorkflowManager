using System.Reflection;
using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core;

internal class StepsMetadata<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    private readonly IDictionary<string, StepMetadata> steps;

    private StepsMetadata(Assembly assembly)
    {
        var baseStepType = typeof(BaseStep<TWorkflow, TWorkflowStep>);
        this.steps =
            assembly.GetTypes()
                .Where(t => !t.IsGenericType && t != baseStepType && baseStepType.IsAssignableFrom(t))
                .ToDictionary(
                    stepType =>
                    {
                        var attribute = stepType.GetCustomAttribute<StepAttribute>();
                        if (attribute is null)
                        {
                            throw new ArgumentException(
                                $"{stepType.FullName} doesn't have StepAttribute",
                                nameof(stepType));
                        }

                        if (attribute.Step == "Empty")
                        {
                            throw new ArgumentException("Step cannot be 'Empty'");
                        }

                        return attribute.Step;
                    },
                    type => new StepMetadata(type));
    }

    public static StepsMetadata<TWorkflow, TWorkflowStep> GetInstance(Assembly assembly)
    {
        return new StepsMetadata<TWorkflow, TWorkflowStep>(assembly);
    }

    public StepMetadata GetStepMeta(string step)
    {
        return this.steps[step];
    }
}