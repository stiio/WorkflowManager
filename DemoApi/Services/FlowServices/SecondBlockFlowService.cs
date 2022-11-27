using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Services.CustomLogic;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class SecondBlockFlowService
{
    private readonly RelatedObjectFlowService relatedObjectFlowService;
    private readonly ThirdBlockFlowService thirdBlockFlowService;

    public SecondBlockFlowService(RelatedObjectFlowService relatedObjectFlowService, ThirdBlockFlowService thirdBlockFlowService)
    {
        this.relatedObjectFlowService = relatedObjectFlowService;
        this.thirdBlockFlowService = thirdBlockFlowService;
    }

    public Task<NextStepResult> CompleteSecondBlockQuestion1(AppWorkflowManager workflowManager)
    {
        var firstBlockCustomLogic = workflowManager.GetLastCustomLogic<IFirstBlockCustomLogic>()!;

        if (firstBlockCustomLogic.IsSomething())
        {
            return Task.FromResult(NextStepResult.Create(Step.SecondBlockQuestion2.ToString()));
        }

        return this.CompleteSecondBlockQuestion4(workflowManager);
    }

    public async Task<NextStepResult> CompleteSecondBlockQuestion4(AppWorkflowManager workflowManager)
    {
        var nextStepKey = await this.relatedObjectFlowService.GetNextStepKey(workflowManager, Step.SecondBlockQuestion4);

        if (nextStepKey != null)
        {
            return NextStepResult.Create(nextStepKey);
        }

        return await thirdBlockFlowService.Start(workflowManager);
    }
}