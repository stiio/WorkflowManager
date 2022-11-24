using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion2))]
public class SecondBlockQuestion2Step : CustomBaseStep<SecondBlockQuestion2Data>,
    INextStep
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new SecondBlockQuestion2Response()
        {
            SomeAnswer = this.Data?.SomeAnswer,
            SomeInput = this.Data?.SomeInput,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(
            NextStepResult.Create(Step.SecondBlockQuestion3.ToString(),
            new SecondBlockQuestion3Payload()
            {
                SomeString = Guid.NewGuid().ToString(),
            }));
    }
}