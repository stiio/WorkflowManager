using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Services.Steps.ThirdBlock;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class ThirdBlockFlowService
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly ReviewBlockService reviewBlockService;
    private readonly RelatedObjectFlowService relatedObjectFlowService;

    public ThirdBlockFlowService(
        ApplicationDbContext applicationDbContext,
        ReviewBlockService reviewBlockService,
        RelatedObjectFlowService relatedObjectFlowService)
    {
        this.applicationDbContext = applicationDbContext;
        this.reviewBlockService = reviewBlockService;
        this.relatedObjectFlowService = relatedObjectFlowService;
    }

    public async Task<NextStepResult> Start(CustomWorkflowManager workflowManager)
    {
        var hasRelatedObjects = await this.applicationDbContext.RelatedObjects
            .AnyAsync(relatedObject => relatedObject.WorkflowId == workflowManager.Workflow.Id);

        if (hasRelatedObjects)
        {
            return NextStepResult.Create(Step.ThirdBlockQuestion1.ToString());
        }

        return await this.reviewBlockService.Start(workflowManager);
    }

    public async Task<NextStepResult> CompleteThirdBlockQuestion2(CustomWorkflowManager workflowManager)
    {
        var relatedObjects = await this.applicationDbContext.RelatedObjects.Where(relatedObject =>
            relatedObject.WorkflowId == workflowManager.Workflow.Id)
            .ToArrayAsync();

        var thirdBlockQuestion2StepKeys = workflowManager.GetStep<ThirdBlockQuestion1Step>()
            ?.Data
            ?.CheckedIds
            .Where(checkedId => relatedObjects.Any(relatedObject => relatedObject.Id == checkedId))
            .Select(checkedId => StepKey.Create(Step.ThirdBlockQuestion2.ToString(), checkedId.ToString()))
            .ToArray();

        var nextThirdBlockQuestion2StepKey = this.relatedObjectFlowService.GetNextKey(workflowManager, thirdBlockQuestion2StepKeys);
        if (nextThirdBlockQuestion2StepKey is not null)
        {
            return NextStepResult.Create(nextThirdBlockQuestion2StepKey);
        }

        return await this.reviewBlockService.Start(workflowManager);
    }
}