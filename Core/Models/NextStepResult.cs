namespace Stio.WorkflowManager.Core.Models;

public class NextStepResult
{
    private NextStepResult()
    {
    }

    public StepKey? StepKey { get; set; }

    public object? Payload { get; set; }

    public static NextStepResult Complete()
    {
        return new NextStepResult();
    }

    public static NextStepResult NextStep(string step, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step),
            Payload = payload
        };
    }

    public static NextStepResult NextStep(string step, string relatedObjectId, object? payload = null)
    {
        return new NextStepResult()
        {
            StepKey = StepKey.Create(step, relatedObjectId),
            Payload = payload,
        };
    }
}