using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/second_block")]
public class SecondBlockController : ControllerBase
{
    private readonly IAppWorkflowManagerFactory workflowManagerFactory;

    public SecondBlockController(IAppWorkflowManagerFactory workflowManagerFactory)
    {
        this.workflowManagerFactory = workflowManagerFactory;
    }

    [HttpGet("first_question")]
    public async Task<ActionResult<SecondBlockQuestion1Response>> GetFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<SecondBlockQuestion1Response>();
    }

    [HttpPatch("first_question")]
    public async Task<ActionResult<NextStepResponse>> EditFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next();

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpGet("second_question")]
    public async Task<ActionResult<SecondBlockQuestion2Response>> GetSecondQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<SecondBlockQuestion2Response>();
    }

    [HttpPatch("second_question")]
    public async Task<ActionResult<NextStepResponse>> EditSecondQuestion(Guid workflowId, [Required] SecondBlockQuestion2Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpGet("third_question")]
    public async Task<ActionResult<SecondBlockQuestion3Response>> GetThirdQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion3Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<SecondBlockQuestion3Response>();
    }

    [HttpPatch("third_question")]
    public async Task<ActionResult<NextStepResponse>> EditThirdQuestion(Guid workflowId, [Required] SecondBlockQuestion3Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion3Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpGet("fourth_question")]
    public async Task<ActionResult<SecondBlockQuestion4Response>> GetFourthQuestion(Guid workflowId, Guid relatedObjectId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion4Step>(relatedObjectId.ToString()))
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<SecondBlockQuestion4Response>();
    }

    [HttpPatch("fourth_question")]
    public async Task<ActionResult<NextStepResponse>> EditFourthQuestion(Guid workflowId, Guid relatedObjectId, [Required] SecondBlockQuestion4Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion4Step>(relatedObjectId.ToString()))
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }
}