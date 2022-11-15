namespace Stio.WorkflowManager.DemoApi.Data.Entities;

public class User
{
    public Guid Id { get; set; }

    public ICollection<Workflow>? Workflows { get; set; }
}