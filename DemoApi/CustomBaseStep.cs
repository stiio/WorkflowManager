using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data.Entities;

namespace Stio.WorkflowManager.DemoApi;

public abstract class CustomBaseStep : BaseStep<Workflow, WorkflowStep>
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}

public abstract class CustomBaseStep<TData> : BaseStep<Workflow, WorkflowStep, TData>
    where TData : class
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}

public abstract class CustomBaseStep<TData, TPayload> : BaseStep<Workflow, WorkflowStep, TData, TPayload>
    where TData : class
    where TPayload : class
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}