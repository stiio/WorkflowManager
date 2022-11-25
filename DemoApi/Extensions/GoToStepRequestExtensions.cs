using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Extensions;

public static class GoToStepRequestExtensions
{
    public static StepKey CreateStepKey(this GoToStepRequest request)
    {
        return StepKey.Create(request.Step.ToString(), request.RelatedObjectId);
    }
}