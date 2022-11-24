namespace Stio.WorkflowManager.DemoApi.Models;

public class ThirdBlockQuestion1Response
{
    public RelatedObjectDto[] RelatedObjects { get; set; } = null!;

    public Guid[] CheckedIds { get; set; } = null!;
}