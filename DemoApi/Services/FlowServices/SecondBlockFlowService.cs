using Stio.WorkflowManager.Core;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data.Entities;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class SecondBlockFlowService
{
    private RelatedObjectFlowService relatedObjectFlowService;

    public SecondBlockFlowService(RelatedObjectFlowService relatedObjectFlowService)
    {
        this.relatedObjectFlowService = relatedObjectFlowService;
    }

    public NextStepResult CompleteSecondBlock(WorkflowManager<Workflow, WorkflowStep> workflowManager, StepKey currentStepKey)
    {
        throw new NotImplementedException();
    }
}