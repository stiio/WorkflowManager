using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services;
using Stio.WorkflowManager.DemoApi.Services.Steps;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/first_block")]
public class FirstBlockController : ControllerBase
{
    private readonly IWorkflowManagerFactory<Workflow, WorkflowStep> workflowManagerFactory;
    private readonly ApplicationDbContext applicationDbContext;
    private readonly UserService userService;

    public FirstBlockController(
        IWorkflowManagerFactory<Workflow, WorkflowStep> workflowManagerFactory,
        ApplicationDbContext applicationDbContext,
        UserService userService)
    {
        this.workflowManagerFactory = workflowManagerFactory;
        this.applicationDbContext = applicationDbContext;
        this.userService = userService;
    }

    [HttpPost("first_question")]
    public async Task<ActionResult<ResponseWithNextStep>> EditFirstQuestion(Guid? workflowId, [Required] FirstBlockQuestion1StepData request)
    {
        if (workflowId is null)
        {
            var workflow = new Workflow()
            {
                UserId = this.userService.UserId,
                Name = "Some name",
            };

            await this.applicationDbContext.Workflows.AddAsync(workflow);
            await this.applicationDbContext.SaveChangesAsync();

            var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflow.Id);

            var nextStepKey = await workflowManager.Start<FirstBlockQuestion1Step, FirstBlockQuestion1StepData>(request);

            return nextStepKey.ToResponseWithNextStep(workflow.Id);

        }
        else
        {
            var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager((Guid)workflowId);

            if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
            {
                return this.BadRequest();
            }

            var nextStepKey = await workflowManager.Next(request);

            return nextStepKey.ToResponseWithNextStep((Guid)workflowId);
        }
    }

    [HttpGet("{workflowId}/first_question")]
    public async Task<ActionResult<FirstBlockQuestion1StepData>> GetFirstQuestion(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager((Guid)workflowId);

        if (!workflowManager.IsLastStep<FirstBlockQuestion1Step>())
        {
            return this.BadRequest();
        }

        return await workflowManager.GetStepData<FirstBlockQuestion1StepData>();
    }
}