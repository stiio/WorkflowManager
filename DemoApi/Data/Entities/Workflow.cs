using Stio.WorkflowManager.DemoApi.Data.Interfaces;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.DemoApi.Data.Entities;

public class Workflow : IWorkflow, ITimeStamp
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public string? Name { get; set; }

    public ICollection<WorkflowStep>? WorkflowSteps { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}