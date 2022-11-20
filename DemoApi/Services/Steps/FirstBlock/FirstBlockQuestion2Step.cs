using Stio.WorkflowManager.Core.Attributes;
using Stio.WorkflowManager.Core.Interfaces;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data.Entities;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;
using Stio.WorkflowManager.DemoApi.Services.CustomLogic;

namespace Stio.WorkflowManager.DemoApi.Services.Steps.FirstBlock;

[Step(nameof(Step.FirstBlockQuestion2))]
public class FirstBlockQuestion2Step : BaseStep<Workflow, WorkflowStep, FirstBlockQuestion2Data>,
    INextStep,
    IFirstBlockCustomLogic
{
    public override Task<object> GetStepData()
    {
        return Task.FromResult<object>(new FirstBlockQuestion2Data()
        {
            Amount = this.Data?.Amount ?? 0,
        });
    }

    public Task<NextStepResult> Next()
    {
        return Task.FromResult(NextStepResult.Create(Step.SecondBlockQuestion1.ToString()));
    }

    public bool IsSomething()
    {
        return this.Data?.Amount > 500;
    }
}