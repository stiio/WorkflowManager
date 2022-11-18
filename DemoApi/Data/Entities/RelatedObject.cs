using Stio.WorkflowManager.DemoApi.Data.Interfaces;

namespace Stio.WorkflowManager.DemoApi.Data.Entities;

public class RelatedObject : ITimeStamp
{
    public Guid Id { get; set; }

    public Guid WorkflowId { get; set; }

    public Workflow? Workflow { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}