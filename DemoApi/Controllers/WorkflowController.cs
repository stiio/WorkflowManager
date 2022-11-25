using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[Route("api/workflows/{workflowId}")]
[ApiController]
public class WorkflowController : ControllerBase
{
    private readonly ICustomWorkflowManagerFactory workflowManagerFactory;

    public WorkflowController(ICustomWorkflowManagerFactory workflowManagerFactory)
    {
        this.workflowManagerFactory = workflowManagerFactory;
    }

    [HttpPatch("previous_step")]
    public async Task<ActionResult<NextStepResponse>> GoToPreviousStep(Guid workflowId)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        var nextStepKey = await workflowManager.GoToPreviousStep();

        return nextStepKey.ToNextStepResponse(workflowId);
    }

    [HttpPatch("step")]
    public async Task<ActionResult<NextStepResponse>> GoToStep(Guid workflowId, [Required] GoToStepRequest request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(workflowId);

        var stepKey = request.CreateStepKey();

        if (!workflowManager.HasStep(stepKey))
        {
            return this.BadRequest("Step not found");
        }

        var nextStep = await workflowManager.GoToStep(stepKey);

        return nextStep.ToNextStepResponse(workflowId);
    }
}