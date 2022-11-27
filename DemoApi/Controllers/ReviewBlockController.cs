using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.ReviewBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/review_block")]
public class ReviewBlockController : ControllerBase
{
    private readonly IAppWorkflowManagerFactory workflowManagerFactory;

    public ReviewBlockController(IAppWorkflowManagerFactory workflowManagerFactory)
    {
        this.workflowManagerFactory = workflowManagerFactory;
    }

    [HttpGet("review")]
    public async Task<ActionResult<ReviewResponse>> GetReview(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ReviewStep>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<ReviewResponse>();
    }

    [HttpPatch("review")]
    public async Task<ActionResult<NextStepResponse>> EditReview(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ReviewStep>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next();

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpGet("completed")]
    public async Task<ActionResult<CompletedResponse>> GetCompleted(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<CompletedStep>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<CompletedResponse>();
    }
}