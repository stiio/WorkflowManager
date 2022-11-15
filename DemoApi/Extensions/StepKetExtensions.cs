using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Extensions;

public static class StepKetExtensions
{
    public static ResponseWithNextStep ToResponseWithNextStep(this StepKey? stepKey, Guid workflowId)
    {
        ArgumentNullException.ThrowIfNull(stepKey);

        return new ResponseWithNextStep()
        {
            WorkflowId = workflowId,
            Step = Enum.Parse<Step>(stepKey.Step),
            RelatedObjectId = stepKey.RelatedObjectId,
        };
    }
}