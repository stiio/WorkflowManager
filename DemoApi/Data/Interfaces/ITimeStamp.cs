namespace Stio.WorkflowManager.DemoApi.Data.Interfaces;

public interface ITimeStamp
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set;}
}