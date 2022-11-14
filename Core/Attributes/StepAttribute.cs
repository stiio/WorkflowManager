namespace Stio.WorkflowManager.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class StepAttribute : Attribute
{
    public string Step { get; set; }

    public StepAttribute(string step)
    {
        ArgumentNullException.ThrowIfNull(step);

        this.Step = step;
    }

    public StepAttribute(Enum step)
    {
        this.Step = step.ToString();
    }
}