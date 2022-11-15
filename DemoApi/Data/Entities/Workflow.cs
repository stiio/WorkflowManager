using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.DemoApi.Data.Entities;

public class Workflow : IWorkflow
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public string? Name { get; set; }

    public ICollection<WorkflowStep>? WorkflowSteps { get; set; }
}