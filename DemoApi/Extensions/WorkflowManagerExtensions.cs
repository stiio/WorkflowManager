using Stio.WorkflowManager.Core;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Extensions;

public static class WorkflowManagerExtensions
{
    public static WrongStepResponse CreateWrongStepResponse(this WorkflowManager<Workflow, WorkflowStep> workflowManager)
    {
        var lastStep = workflowManager.GetLastStep();

        return new WrongStepResponse()
        {
            WorkflowId = workflowManager.Workflow.Id,
            Step = Enum.Parse<Step>(lastStep.StepKey.Step),
            RelatedObjectId = lastStep.StepKey.RelatedObjectId,
        };
    }
}