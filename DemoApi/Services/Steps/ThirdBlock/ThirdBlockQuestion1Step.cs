using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.ThirdBlock;

[Step(nameof(Step.ThirdBlockQuestion1))]
public class ThirdBlockQuestion1Step : AppBaseStep<ThirdBlockQuestion1Data>,
    INextStep
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly ThirdBlockFlowService thirdBlockFlowService;

    public ThirdBlockQuestion1Step(
        ApplicationDbContext applicationDbContext,
        ThirdBlockFlowService thirdBlockFlowService)
    {
        this.applicationDbContext = applicationDbContext;
        this.thirdBlockFlowService = thirdBlockFlowService;
    }

    public override async Task<object> GetStepData()
    {
        var relatedObjects = await this.applicationDbContext.RelatedObjects
            .Where(relatedObject => relatedObject.WorkflowId == this.WorkflowManager.WorkflowId)
            .OrderByDescending(relatedObject => relatedObject.CreatedAt)
            .ToArrayAsync();

        return new ThirdBlockQuestion1Response()
        {
            RelatedObjects = relatedObjects.Select(relatedObject => new RelatedObjectDto()
            {
                Id = relatedObject.Id,
                Name = relatedObject.Name,
            }).ToArray(),
            CheckedIds = this.Data?.CheckedIds ?? Array.Empty<Guid>(),
        };
    }

    public Task<NextStepResult> Next()
    {
        return this.thirdBlockFlowService.CompleteThirdBlockQuestion2(this.WorkflowManager);
    }
}