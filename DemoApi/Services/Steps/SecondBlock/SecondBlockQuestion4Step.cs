using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion4))]
public class SecondBlockQuestion4Step : BaseStep<Workflow, WorkflowStep, SecondBlockQuestion4Data>,
    INextStep
{
    private readonly ApplicationDbContext applicationDbContext;

    public SecondBlockQuestion4Step(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public override async Task<object> GetStepData()
    {
        var relatedObject = await this.applicationDbContext.RelatedObjects.FindAsync(new Guid(this.StepKey.RelatedObjectId!));

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
        // TODO: Start third block
        throw new NotImplementedException();
    }
}