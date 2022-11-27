using Stio.WorkflowManager.Core.Extensions;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Models;

/// <summary>
/// Base step
/// </summary>
/// <typeparam name="TWorkflow">Implementation of IWorkflow</typeparam>
/// <typeparam name="TWorkflowStep">Implementation of IWorkflowStep</typeparam>
public abstract class BaseStep<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
{
    /// <summary>
    /// StepKey of this step
    /// </summary>
    public StepKey StepKey { get; internal set; } = null!;

    /// <summary>
    /// StepKey of previous step
    /// </summary>
    public StepKey? PreviousStepKey { get; internal set; }

    /// <summary>
    /// Related WorkflowManager
    /// </summary>
    protected internal WorkflowManager<TWorkflow, TWorkflowStep> WorkflowManager { get; internal set; } = null!;

    /// <summary>
    /// Related WorkflowStep
    /// </summary>
    protected internal TWorkflowStep WorkflowStep { get; internal set; } = null!;

    /// <summary>
    /// Return current step data.
    /// </summary>
    /// <returns></returns>
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

/// <summary>
/// Base step with data
/// </summary>
/// <typeparam name="TWorkflow">Implementation of IWorkflow</typeparam>
/// <typeparam name="TWorkflowStep">Implementation of IWorkflowStep</typeparam>
/// <typeparam name="TData">Data type of step</typeparam>
public abstract class BaseStep<TWorkflow, TWorkflowStep, TData> : BaseStep<TWorkflow, TWorkflowStep>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
    where TData : class
{
    /// <summary>
    /// Data of step
    /// </summary>
    public TData? Data { get; set; }

    internal override TWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetData(this.Data);

        return this.WorkflowStep;
    }
}

/// <summary>
/// Base step with data and payload
/// </summary>
/// <typeparam name="TWorkflow">Implementation of IWorkflow</typeparam>
/// <typeparam name="TWorkflowStep">Implementation of IWorkflowStep</typeparam>
/// <typeparam name="TData">Data type of step</typeparam>
/// <typeparam name="TPayload">Payload type of step</typeparam>
public abstract class BaseStep<TWorkflow, TWorkflowStep, TData, TPayload> : BaseStep<TWorkflow, TWorkflowStep, TData>
    where TWorkflow : class, IWorkflow
    where TWorkflowStep : class, IWorkflowStep
    where TData : class
    where TPayload : class
{
    /// <summary>
    /// Payload of step
    /// </summary>
    public TPayload? Payload;

    internal override TWorkflowStep UpdateWorkflowStep()
    {
        base.UpdateWorkflowStep();

        this.WorkflowStep.SetPayload(this.Payload);

        return this.WorkflowStep;
    }
}