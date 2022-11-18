using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion1))]
public class SecondBlockQuestion1Step : BaseStep<Workflow, WorkflowStep>,
    INextStep
{
    private readonly UserService userService;
    private readonly ApplicationDbContext applicationDbContext;

    public SecondBlockQuestion1Step(UserService userService, ApplicationDbContext applicationDbContext)
    {
        this.userService = userService;
        this.applicationDbContext = applicationDbContext;
    }

    public override async Task<object> GetStepData()
    {
        var firsBlockQuestion1Data = this.WorkflowManager.GetStep<FirstBlockQuestion1Step>()?.Data;

        return new SecondBlockQuestion1Response()
        {
            FullName = $"{firsBlockQuestion1Data?.FirstName} {firsBlockQuestion1Data?.LastName}".Trim(),
            UserId = this.userService.UserId,
            RelatedObjects = await this.applicationDbContext.RelatedObjects
                .Where(relatedObject => relatedObject.WorkflowId == this.WorkflowManager.Workflow.Id)
                .Select(relatedObject => new RelatedObjectDto()
                {
                    Id = relatedObject.Id,
                    Name = relatedObject.Name,
                })
                .OrderBy(relatedObject => relatedObject.Name)
                .ToArrayAsync()

        };
    }

    public async Task<NextStepResult> Next()
    {
        throw new NotImplementedException();
    }
}