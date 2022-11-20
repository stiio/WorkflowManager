using Stio.WorkflowManager.Core;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Services.CustomLogic;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class SecondBlockFlowService
{
    private readonly RelatedObjectFlowService relatedObjectFlowService;

    public SecondBlockFlowService(RelatedObjectFlowService relatedObjectFlowService, ApplicationDbContext applicationDbContext)
    {
        this.relatedObjectFlowService = relatedObjectFlowService;
    }

    public Task<NextStepResult> CompleteSecondBlockQuestion1(WorkflowManager<Workflow, WorkflowStep> workflowManager)
    {
        var firstBlockCustomLogic = workflowManager.GetLastCustomLogic<IFirstBlockCustomLogic>()!;

        if (firstBlockCustomLogic.IsSomething())
        {
            return Task.FromResult(NextStepResult.Create(Step.SecondBlockQuestion2.ToString()));
        }

        return this.CompleteSecondBlockQuestion4(workflowManager);
    }

    public async Task<NextStepResult> CompleteSecondBlockQuestion4(WorkflowManager<Workflow, WorkflowStep> workflowManager)
    {
        var nextStepKey = await this.relatedObjectFlowService.GetNextStepKey(workflowManager, Step.SecondBlockQuestion4);

        if (nextStepKey != null)
        {
            return NextStepResult.Create(nextStepKey);
        }

        // TODO: Start third block
        throw new NotImplementedException();
    }
}