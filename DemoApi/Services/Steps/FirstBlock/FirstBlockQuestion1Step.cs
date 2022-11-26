using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

[Step(nameof(Step.FirstBlockQuestion1))]
public class FirstBlockQuestion1Step : CustomBaseStep<FirstBlockQuestion1Data>,
    INextStep
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion1Response()
        {
            FirstName = this.Data?.FirstName,
            LastName = this.Data?.LastName,
            Agree = this.Data?.Agree,
        });
    }

    public Task<NextStepResult> Next()
    {
        if (this.Data!.Agree)
        {
            return Task.FromResult(NextStepResult.Create(Step.FirstBlockQuestion2.ToString()));
        }

        return Task.FromResult(NextStepResult.Create(Step.FirstBlockQuestion3.ToString()));
    }
}