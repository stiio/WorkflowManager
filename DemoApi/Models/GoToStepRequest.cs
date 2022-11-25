using Stio.WorkflowManager.DemoApi.Enums;

namespace Stio.WorkflowManager.DemoApi.Models;

public class GoToStepRequest
{
    public Step Step { get; set; }

    public string? RelatedObjectId { get; set; }
}