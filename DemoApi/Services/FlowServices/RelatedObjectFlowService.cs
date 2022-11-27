using Microsoft.EntityFrameworkCore;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data;
using Stio.WorkflowManager.DemoApi.Enums;

namespace Stio.WorkflowManager.DemoApi.Services.FlowServices;

public class RelatedObjectFlowService
{
    private readonly ApplicationDbContext applicationDbContext;

    public RelatedObjectFlowService(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<StepKey?> GetNextStepKey(AppWorkflowManager workflowManager, Step step)
    {
        var relatedObjects = await this.applicationDbContext.RelatedObjects
            .Where(relatedObject => relatedObject.WorkflowId == workflowManager.Workflow.Id)
            .ToArrayAsync();

        return relatedObjects
            .OrderBy(relatedObject => relatedObject.Name)
            .Select(relatedObject => StepKey.Create(step.ToString(), relatedObject.Id.ToString()))
            .FirstOrDefault(stepKey => !workflowManager.HasStep(stepKey));
    }

    public StepKey? GetNextKey(AppWorkflowManager workflowManager, StepKey[] stepKeys)
    {
        return stepKeys.FirstOrDefault(k => !workflowManager.HasStep(k));
    }
}