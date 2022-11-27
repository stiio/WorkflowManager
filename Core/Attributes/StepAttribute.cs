namespace Stio.WorkflowManager.Core.Attributes;

/// <summary>
/// Step Attribute (required for implementation of BaseStep)
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class StepAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StepAttribute"/> class.
    /// </summary>
    /// <param name="step">Step identifier</param>
    public StepAttribute(string step)
    {
        ArgumentNullException.ThrowIfNull(step);

        this.Step = step;
    }

    /// <summary>
    /// Step identifier
    /// </summary>
    public string Step { get; set; }
}