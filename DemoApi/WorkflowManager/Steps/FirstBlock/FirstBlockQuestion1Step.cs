using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.WorkflowManager.Steps.FirstBlock;

[Step(nameof(Step.FirstBlockQuestion1))]
public class FirstBlockQuestion1Step : BaseStep<FirstBlockQuestion1StepData>,
    INextStep
{
    public override Task<object?> GetStepData()
    {
        return Task.FromResult<object?>(new FirstBlockQuestion1StepData()
        {
            FirstName = this.Data?.FirstName,
            LastName = this.Data?.LastName,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.NextStep(Step.FirstBlockQuestion2.ToString()));
    }
}