using Stio.WorkflowManager.Store.Enum;

namespace Stio.WorkflowManager.Store.Entity;

public class Workflow
{
    public Guid Id { get; set; }

    public WorkflowStatus Status { get; set; }
}