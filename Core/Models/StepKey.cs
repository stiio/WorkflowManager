namespace Stio.WorkflowManager.Core.Models;

public record StepKey
{
    private StepKey()
    {
    }

    public string Step { get; set; } = null!;

    public string? RelatedObjectId { get; set; }

    public static StepKey Create(string step, string? relatedObjectId = null)
    {
        ArgumentNullException.ThrowIfNull(step);

        return new StepKey() { Step = step, RelatedObjectId = relatedObjectId };
    }
}