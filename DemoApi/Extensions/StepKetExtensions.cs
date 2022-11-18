using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Extensions;

public static class StepKetExtensions
{
    public static NextStepResponse ToNextStepResponse(this StepKey stepKey, Guid workflowId)
    {
        return new NextStepResponse()
        {
            WorkflowId = workflowId,
            Step = Enum.Parse<Step>(stepKey.Step),
            RelatedObjectId = stepKey.RelatedObjectId,
        };
    }
}