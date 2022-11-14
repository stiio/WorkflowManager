using System.Text.Json;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core.Extensions;

internal static class WorkflowStepExtensions
{
    public static void SetStepKey(this WorkflowStep workflowStep, StepKey stepKey)
    {
        workflowStep.StepKey = JsonSerializer.Serialize(stepKey);
    }

    public static void SetPreviousStepKey(this WorkflowStep workflowStep, StepKey? stepKey)
    {
        workflowStep.PreviousStepKey = JsonSerializer.Serialize(stepKey);
    }

    public static void SetData<TData>(this WorkflowStep workflowStep, TData? data)
    {
        workflowStep.Data = JsonSerializer.Serialize(data);
    }

    public static void SetPayload<TPayload>(this WorkflowStep workflowStep, TPayload? payload)
    {
        workflowStep.Payload = JsonSerializer.Serialize(payload);
    }
}