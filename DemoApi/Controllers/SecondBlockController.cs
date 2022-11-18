using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;
using Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/workflows/{workflowId}/second_block")]
public class SecondBlockController : ControllerBase
{
    private readonly IWorkflowManagerFactory<Workflow, WorkflowStep> workflowManagerFactory;
    private readonly ApplicationDbContext applicationDbContext;

    public SecondBlockController(
        IWorkflowManagerFactory<Workflow, WorkflowStep> workflowManagerFactory,
        ApplicationDbContext applicationDbContext)
    {
        this.workflowManagerFactory = workflowManagerFactory;
        this.applicationDbContext = applicationDbContext;
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
}