using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;
using Stio.WorkflowManager.DemoApi.Services.Steps.ThirdBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/third_block")]
public class ThirdBlockController : ControllerBase
{
    private readonly ICustomWorkflowManagerFactory workflowManagerFactory;

    public ThirdBlockController(ICustomWorkflowManagerFactory workflowManagerFactory)
    {
        this.workflowManagerFactory = workflowManagerFactory;
    }

    [HttpGet("first_question")]
    public async Task<ActionResult<ThirdBlockQuestion1Response>> GetFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ThirdBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<ThirdBlockQuestion1Response>();
    }

    [HttpPatch("first_question")]
    public async Task<ActionResult<NextStepResponse>> EditFirstQuestion(Guid workflowId, [Required] ThirdBlockQuestion1Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ThirdBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStep = await workflowManager.Next(request);

        return nextStep.ToNextStepResponse(workflowId);
    }

    [HttpGet("second_question")]
    public async Task<ActionResult<ThirdBlockQuestion2Response>> GetSecondQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ThirdBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<ThirdBlockQuestion2Response>();
    }

    [HttpPatch("second_question")]
    public async Task<ActionResult<NextStepResponse>> EditSecondQuestion(Guid workflowId, [Required] ThirdBlockQuestion2Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<ThirdBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStep = await workflowManager.Next(request);

        return nextStep.ToNextStepResponse(workflowId);
    }
}