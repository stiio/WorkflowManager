namespace Stio.WorkflowManager.Store.Entity;

public interface IWorkflowStep
{
    public Guid Id { get; set; }

    public Guid WorkflowId { get; set; }

    public string? Data { get; set; }

    public string? Payload { get; set; }

    public string? StepKey { get; set; }

    public string? PreviousStepKey { get; set; }

    public bool IsSoftDelete { get; set; }
}