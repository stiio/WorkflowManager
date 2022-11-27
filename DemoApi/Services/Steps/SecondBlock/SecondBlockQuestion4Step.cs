using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.FlowServices;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion4))]
public class SecondBlockQuestion4Step : AppBaseStep<SecondBlockQuestion4Data>,
    INextStep
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly SecondBlockFlowService secondBlockFlowService;

    public SecondBlockQuestion4Step(
        ApplicationDbContext applicationDbContext,
        SecondBlockFlowService secondBlockFlowService)
    {
        this.applicationDbContext = applicationDbContext;
        this.secondBlockFlowService = secondBlockFlowService;
    }

    public override async Task<object> GetStepData()
    {
        var relatedObject = await this.applicationDbContext.RelatedObjects.FindAsync(this.GetRelatedObjectId());

        ArgumentNullException.ThrowIfNull(relatedObject);

        return new SecondBlockQuestion4Response()
        {
            Name = relatedObject.Name,
            Amount = this.Data?.Amount,
            Answer = this.Data?.Answer,
        };
    }

    public Task<NextStepResult> Next()
    {
        return this.secondBlockFlowService.CompleteSecondBlockQuestion4(this.WorkflowManager);
    }
}