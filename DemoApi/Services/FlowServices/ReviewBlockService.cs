using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class ReviewBlockService
{
    private readonly ApplicationDbContext applicationDbContext;

    public ReviewBlockService(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public Task<NextStepResult> Start(CustomWorkflowManager workflowManager)
    {
        return Task.FromResult(NextStepResult.Create(Step.Review.ToString()));
    }

    public Task<ReviewResponse> CreateReviewResponse(CustomWorkflowManager workflowManager)
    {
        return Task.FromResult(new ReviewResponse());
    }
}