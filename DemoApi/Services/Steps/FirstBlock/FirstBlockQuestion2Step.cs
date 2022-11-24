using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.CustomLogic;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

[Step(nameof(Step.FirstBlockQuestion2))]
public class FirstBlockQuestion2Step : CustomBaseStep<FirstBlockQuestion2Data>,
    INextStep,
    IFirstBlockCustomLogic
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion2Response()
        {
            Amount = this.Data?.Amount,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.Create(Step.SecondBlockQuestion1.ToString()));
    }

    public bool IsSomething()
    {
        return this.Data?.Amount > 500;
    }
}