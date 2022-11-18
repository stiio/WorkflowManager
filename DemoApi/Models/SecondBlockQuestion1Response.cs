namespace Stio.WorkflowManager.DemoApi.Models;

public class SecondBlockQuestion1Response
{
    public Guid UserId { get; set; }

    public string? FullName { get; set; }

    public RelatedObjectDto[] RelatedObjects { get; set; } = null!;
}