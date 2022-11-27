using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.DemoApi.Data.Entities;

#pragma warning disable SA1402 // File may only contain a single type

namespace Stio.WorkflowManager.DemoApi;

public abstract class AppBaseStep : BaseStep<Workflow, WorkflowStep>
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}

public abstract class AppBaseStep<TData> : BaseStep<Workflow, WorkflowStep, TData>
    where TData : class
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}

public abstract class AppBaseStep<TData, TPayload> : BaseStep<Workflow, WorkflowStep, TData, TPayload>
    where TData : class
    where TPayload : class
{
    public Guid GetRelatedObjectId()
    {
        return new Guid(this.StepKey.RelatedObjectId!);
    }
}