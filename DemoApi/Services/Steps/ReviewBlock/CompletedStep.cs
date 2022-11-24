using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.ReviewBlock;

[Step(nameof(Step.Completed))]
public class CompletedStep : CustomBaseStep
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new CompletedResponse()
        {
            Message = "Workflow completed.",
        });
    }
}