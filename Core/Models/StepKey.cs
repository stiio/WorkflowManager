namespace Stio.WorkflowManager.Core.Models;

/// <summary>
/// Step Key
/// </summary>
public record StepKey
{
    /// <summary>
    /// Unique identifier of step
    /// </summary>
    public string Step { get; set; } = null!;

    /// <summary>
    /// Id of related object for step
    /// </summary>
    public string? RelatedObjectId { get; set; }

    /// <summary>
    /// Create <see cref="StepKey"/>
    /// </summary>
    /// <param name="step">Unique identifier of step</param>
    /// <param name="relatedObjectId">Id of related object for step</param>
    /// <returns></returns>
    public static StepKey Create(string step, string? relatedObjectId = null)
    {
        ArgumentNullException.ThrowIfNull(step);

        return new StepKey() { Step = step, RelatedObjectId = relatedObjectId };
    }
}