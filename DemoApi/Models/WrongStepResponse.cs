using Stio.WorkflowManager.DemoApi.Enums;

namespace Stio.WorkflowManager.DemoApi.Models;

public class WrongStepResponse
{
    public Guid WorkflowId { get; set; }

    public Step Step { get; set; }

    public string? RelatedObjectId { get; set; }
}