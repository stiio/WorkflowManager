using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.ReviewBlock;

[Step(nameof(Step.Review))]
public class ReviewStep : AppBaseStep,
    INextStep
{
    private readonly ReviewBlockService reviewBlockService;

    public ReviewStep(ReviewBlockService reviewBlockService)
    {
        this.reviewBlockService = reviewBlockService;
    }

    public override async Task<object> GetStepData()
    {
        return await this.reviewBlockService.CreateReviewResponse(this.WorkflowManager);
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.Create(Step.Completed.ToString()));
    }
}