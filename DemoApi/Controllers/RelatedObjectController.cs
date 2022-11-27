using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Extensions;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

namespace Stio.WorkflowManager.DemoApi.Controllers;

[ApiController]
[Route("api/related_objects")]
public class RelatedObjectController : ControllerBase
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly IAppWorkflowManagerFactory workflowManagerFactory;

    public RelatedObjectController(
        ApplicationDbContext applicationDbContext,
        IAppWorkflowManagerFactory workflowManagerFactory)
    {
        this.applicationDbContext = applicationDbContext;
        this.workflowManagerFactory = workflowManagerFactory;
    }

    [HttpPost]
    public async Task<ActionResult<RelatedObjectDto>> CreateRelatedObject([Required] RelatedObjectCreateRequest request)
    {
        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(request.WorkflowId);

        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        var relatedObject = new RelatedObject()
        {
            WorkflowId = request.WorkflowId,
            Name = request.Name,
        };

        await this.applicationDbContext.RelatedObjects.AddAsync(relatedObject);
        await this.applicationDbContext.SaveChangesAsync();

        return new RelatedObjectDto()
        {
            Id = relatedObject.Id,
            Name = relatedObject.Name,
        };
    }

    [HttpGet("{relatedObjectId}")]
    public async Task<ActionResult<RelatedObjectDto>> GetRelatedObject(Guid relatedObjectId)
    {
        var relatedObject = await applicationDbContext.RelatedObjects.FindAsync(relatedObjectId);

        if (relatedObject == null)
        {
            return this.NotFound();
        }

        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(relatedObject.WorkflowId);
        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        return new RelatedObjectDto()
        {
            Id = relatedObject.Id,
            Name = relatedObject.Name,
        };
    }

    [HttpPost("{relatedObjectId}")]
    public async Task<ActionResult<RelatedObjectDto>> UpdateRelatedObject(Guid relatedObjectId, [Required] RelatedObjectUpdateRequest request)
    {
        var relatedObject = await applicationDbContext.RelatedObjects.FindAsync(relatedObjectId);

        if (relatedObject == null)
        {
            return this.NotFound();
        }

        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(relatedObject.WorkflowId);
        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        relatedObject.Name = request.Name;

        this.applicationDbContext.RelatedObjects.Update(relatedObject);
        await this.applicationDbContext.SaveChangesAsync();

        return new RelatedObjectDto()
        {
            Id = relatedObject.Id,
            Name = relatedObject.Name,
        };
    }

    [HttpDelete("{relatedObjectId}")]
    public async Task<ActionResult> DeleteRelatedObject(Guid relatedObjectId)
    {
        var relatedObject = await applicationDbContext.RelatedObjects.FindAsync(relatedObjectId);

        if (relatedObject == null)
        {
            return this.NotFound();
        }

        var workflowManager = await this.workflowManagerFactory.CreateWorkflowManager(relatedObject.WorkflowId);
        if (!workflowManager.IsLastStep<SecondBlockQuestion1Step>())
        {
            return this.BadRequest(workflowManager.CreateWrongStepResponse());
        }

        this.applicationDbContext.Remove(relatedObject);
        await this.applicationDbContext.SaveChangesAsync();

        return this.Ok();
    }
}