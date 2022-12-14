using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.Core;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services;
using Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/first_block")]
public class FirstBlockController : ControllerBase
{
    private readonly IAppWorkflowManagerFactory workflowManagerFactory;
    private readonly ApplicationDbContext applicationDbContext;
    private readonly UserService userService;

    public FirstBlockController(
        IAppWorkflowManagerFactory workflowManagerFactory,
        ApplicationDbContext applicationDbContext,
        UserService userService)
    {
        this.workflowManagerFactory = workflowManagerFactory;
        this.applicationDbContext = applicationDbContext;
        this.userService = userService;
    }

    [HttpGet("first_question")]
    public async Task<ActionResult<FirstBlockQuestion1Response>> GetFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager((Guid)workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<FirstBlockQuestion1Response>();
    }

    [HttpPost("~/api/workflows/first_block/first_question")]
    public async Task<ActionResult<NextStepResponse>> EditFirstQuestion(Guid? workflowId, [Required] FirstBlockQuestion1Data request)
    {
        WorkflowManager<Workflow, WorkflowStep> workflowManager;
        if (workflowId is null)
        {
            var workflow = new Workflow()
            {
                UserId = this.userService.UserId,
                Name = "Some name",
            };

            await this.applicationDbContext.Workflows.AddAsync(workflow);
            await this.applicationDbContext.SaveChangesAsync();

            workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflow.Id);
        }
        else
        {
            workflowManager = await this.workflowManagerFactory.CreateWorkflowManager((Guid)workflowId);
        }

        StepKey nextStepKey;
        if (workflowManager.HasSteps())
        {
            if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
            {
                return this.BadRequest(workflowManager.CreateWrongStepResponse());
            }

            nextStepKey = await workflowManager.Next(request);
        }
        else
        {
            nextStepKey = await workflowManager.Start<FirstBlockQuestion1Step, FirstBlockQuestion1Data>(request);
        }

        return this.Ok(nextStepKey.ToNextStepResponse(workflowManager.Workflow.Id));
    }

    [HttpGet("second_question")]
    public async Task<ActionResult<FirstBlockQuestion2Response>> GetSecondQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<FirstBlockQuestion2Response>();
    }

    [HttpPatch("second_question")]
    public async Task<ActionResult<NextStepResponse>> EditSecondQuestion(Guid workflowId, [Required] FirstBlockQuestion2Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion2Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpGet("third_question")]
    public async Task<ActionResult<FirstBlockQuestion3Response>> GetThirdQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion3Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return await workflowManager.GetStepData<FirstBlockQuestion3Response>();
    }

    [HttpPatch("third_question")]
    public async Task<ActionResult<NextStepResponse>> EditThirdQuestion(Guid workflowId, [Required] FirstBlockQuestion3Data request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion3Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var nextStepKey = await workflowManager.Next(request);

        return nextStepKey.ToNextStepResponse(workflowId);
    }
}