using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion3))]
public class SecondBlockQuestion3Step : CustomBaseStep<SecondBlockQuestion3Data, SecondBlockQuestion3Payload>,
    INextStep
{
    private readonly SecondBlockFlowService secondBlockFlowService;

    public SecondBlockQuestion3Step(SecondBlockFlowService secondBlockFlowService)
    {
        this.secondBlockFlowService = secondBlockFlowService;
    }

    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new SecondBlockQuestion3Response()
        {
            DataFromPayload = this.Payload?.SomeString,
            Options = new string[] { "1", "2", "3" },
            Checked = this.Data?.Checked ?? Array.Empty<string>(),
        });
    }

    public Task<NextStepResult> Next()
    {
        return this.secondBlockFlowService.CompleteSecondBlockQuestion4(this.WorkflowManager);
    }
}