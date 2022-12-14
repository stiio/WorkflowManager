using Stio.WorkflowManager.DemoApi.Data.Interfaces;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.DemoApi.Data.Entities;

public class WorkflowStep : IWorkflowStep, ITimeStamp
{
    public Guid Id { get; set; }

    public Workflow? Workflow { get; set; }

    public Guid WorkflowId { get; set; }

    public string? Data { get; set; }

    public string? Payload { get; set; }

    public string? StepKey { get; set; }

    public string? PreviousStepKey { get; set; }

    public bool IsSoftDelete { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}