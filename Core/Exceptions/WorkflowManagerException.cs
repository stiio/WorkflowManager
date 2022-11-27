namespace Stio.WorkflowManager.Core.Exceptions;

/// <inheritdoc />
public class WorkflowManagerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowManagerException"/> class.
    /// </summary>
    /// <param name="message">Message</param>
    public WorkflowManagerException(string message)
        : base(message)
    {
    }
}