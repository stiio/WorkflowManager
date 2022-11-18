using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.SecondBlock;

[Step(nameof(Step.SecondBlockQuestion1))]
public class SecondBlockQuestion2Step : BaseStep<Workflow, WorkflowStep>,
    INextStep
{
    private readonly UserService userService;

    public SecondBlockQuestion2Step(UserService userService)
    {
        this.userService = userService;
    }

    public override Task<object> GetStepData()
    {
        var firsBlockQuestion1Data = this.WorkflowManager.GetStep<FirstBlockQuestion1Step>()?.Data;

        return Task.FromResult<object>(new SecondBlockQuestion1Response()
        {
            FullName = $"{firsBlockQuestion1Data?.FirstName} {firsBlockQuestion1Data?.LastName}".Trim(),
            UserId = this.userService.UserId,
        });
    }

    public async Task<NextStepResult> Next()
    {
        throw new NotImplementedException();
    }
}