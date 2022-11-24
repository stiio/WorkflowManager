using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.CustomLogic;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

[Step(nameof(Step.FirstBlockQuestion3))]
public class FirstBlockQuestion3Step : CustomBaseStep<FirstBlockQuestion3Data>,
    INextStep,
    IFirstBlockCustomLogic
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion3Response()
        {
            Agree = this.Data?.Agree ?? false,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.Create(Step.SecondBlockQuestion1.ToString()));
    }

    public bool IsSomething()
    {
        return this.Data?.Agree ?? false;
    }
}