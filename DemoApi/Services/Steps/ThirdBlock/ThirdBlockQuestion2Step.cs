using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.ThirdBlock;

[Step(nameof(Step.ThirdBlockQuestion2))]
public class ThirdBlockQuestion2Step : CustomBaseStep<ThirdBlockQuestion2Data>,
    INextStep
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly ThirdBlockFlowService thirdBlockFlowService;

    public ThirdBlockQuestion2Step(ApplicationDbContext applicationDbContext, ThirdBlockFlowService thirdBlockFlowService)
    {
        this.applicationDbContext = applicationDbContext;
        this.thirdBlockFlowService = thirdBlockFlowService;
    }

    public override async Task<object> GetStepData()
    {
        var relatedObject = await this.applicationDbContext.RelatedObjects.FindAsync(this.GetRelatedObjectId());

        return new ThirdBlockQuestion2Response()
        {
            Name = relatedObject?.Name,
            Agree = this.Data?.Agree,
            Count = this.Data?.Count,
        };
    }

    public Task<NextStepResult> Next()
    {
        return this.thirdBlockFlowService.CompleteThirdBlockQuestion2(this.WorkflowManager);
    }
}