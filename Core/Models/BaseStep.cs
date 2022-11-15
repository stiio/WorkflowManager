using Stio.WorkflowManager.Core.Extensions;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Models;

public abstract class BaseStep
{
    public StepKey StepKey { get; set; } = null!;

    public StepKey? PreviousStepKey { get; set; }

    protected internal WorkflowManager WorkflowManager { get; set; } = null!;

    protected internal IWorkflowStep WorkflowStep { get; set; } = null!;

    public abstract Task<object?> GetStepData();

    internal static void InitStep(BaseStep step, WorkflowManager workflowManager, IWorkflowStep workflowStep, StepKey stepKey, StepKey? previousStepKey)
    {
        step.WorkflowManager = workflowManager;
        step.WorkflowStep = workflowStep;
        step.StepKey = stepKey;
        step.PreviousStepKey = stepKey;
    }

    internal virtual IWorkflowStep UpdateWorkflowStep()
    {
        this.WorkflowStep.SetStepKey(this.StepKey);
        this.WorkflowStep.SetPreviousStepKey(this.PreviousStepKey);

        return this.WorkflowStep;
    }
}

public abstract class BaseStep<TData> : BaseStep
    where TData : class
{
    public TData? Data { get; set; }

    internal override IWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetData(this.Data);

        return this.WorkflowStep;
    }
}

public abstract class BaseStep<TData, TPayload> : BaseStep<TData>
    where TData : class
    where TPayload : class
{
    public TPayload? Payload;

    internal override IWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetPayload(this.Payload);

        return this.WorkflowStep;
    }
}