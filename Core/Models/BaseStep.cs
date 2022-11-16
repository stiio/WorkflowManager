using Stio.WorkflowManager.Core.Extensions;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Models;

public abstract class BaseStep<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    public StepKey StepKey { get; set; } = null!;

    public StepKey? PreviousStepKey { get; set; }

    protected internal WorkflowManager<TWorkflow, TWorkflowStep> WorkflowManager { get; set; } = null!;

    protected internal TWorkflowStep WorkflowStep { get; set; } = null!;

    public abstract Task<object> GetStepData();

    internal static void InitStep(
        BaseStep<TWorkflow, TWorkflowStep> step,
        WorkflowManager<TWorkflow, TWorkflowStep> workflowManager,
        TWorkflowStep workflowStep,
        StepKey stepKey,
        StepKey? previousStepKey)
    {
        step.WorkflowManager = workflowManager;
        step.WorkflowStep = workflowStep;
        step.StepKey = stepKey;
        step.PreviousStepKey = previousStepKey;
    }

    internal virtual TWorkflowStep UpdateWorkflowStep()
    {
        this.WorkflowStep.SetStepKey(this.StepKey);
        this.WorkflowStep.SetPreviousStepKey(this.PreviousStepKey);

        return this.WorkflowStep;
    }
}

public abstract class BaseStep<TWorkflow, TWorkflowStep, TData> : BaseStep<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
    where TData : class
{
    public TData? Data { get; set; }

    internal override TWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetData(this.Data);

        return this.WorkflowStep;
    }
}

public abstract class BaseStep<TWorkflow, TWorkflowStep, TData, TPayload> : BaseStep<TWorkflow, TWorkflowStep, TData>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
    where TData : class
    where TPayload : class
{
    public TPayload? Payload;

    internal override TWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetPayload(this.Payload);

        return this.WorkflowStep;
    }
}