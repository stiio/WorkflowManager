namespace Stio.WorkflowManager.Core.Models;

/// <summary>
/// NextStepResult
/// </summary>
public class NextStepResult
{
    private NextStepResult()
    {
    }

    /// <summary>
    /// Next step key
    /// </summary>
    public StepKey StepKey { get; set; } = null!;

    /// <summary>
    /// Payload for next step
    /// </summary>
    public object? Payload { get; set; }

    /// <summary>
    /// Create <see cref="NextStepResult"/>
    /// </summary>
    /// <param name="step">Identifier of next step</param>
    /// <param name="payload">Payload for next step</param>
    /// <returns></returns>
    public static NextStepResult Create(string step, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step),
            Payload = payload
        };
    }

    /// <summary>
    /// Create <see cref="NextStepResult"/>
    /// </summary>
    /// <param name="step">Identifier of next step</param>
    /// <param name="relatedObjectId">Id of related object for next step</param>
    /// <param name="payload">Payload for next step</param>
    /// <returns></returns>
    public static NextStepResult Create(string step, string relatedObjectId, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step, relatedObjectId),
            Payload = payload,
        };
    }

    /// <summary>
    /// Create <see cref="NextStepResult"/>
    /// </summary>
    /// <param name="stepKey"><see cref="Models.StepKey"/> of next step</param>
    /// <param name="payload">Payload for next step</param>
    /// <returns></returns>
    public static NextStepResult Create(StepKey stepKey, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = stepKey,
            Payload = payload,
        };
    }
}