namespace Stio.WorkflowManager.Core.Models;

public class NextStepResult
{
    private NextStepResult()
    {
    }

    public StepKey StepKey { get; set; } = null!;

    public object? Payload { get; set; }

    public static NextStepResult Create(string step, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step),
            Payload = payload
        };
    }

    public static NextStepResult Create(string step, string relatedObjectId, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step, relatedObjectId),
            Payload = payload,
        };
    }
}