using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Stio.WorkflowManager.Core.Models;
using Stio.WorkflowManager.Store.Entity;

namespace Stio.WorkflowManager.Core;

internal class StepMetadata
{
    private readonly PropertyInfo? dataProperty;
    private readonly PropertyInfo? payloadProperty;
    private readonly Type type;

    public StepMetadata(Type type)
    {
        this.type = type;

        this.dataProperty = type.GetProperty("Data");
        this.payloadProperty = type.GetProperty("Payload");

        switch (type.BaseType!.Name)
        {
            case "BaseStep`2":
                this.DataType = type.BaseType.GenericTypeArguments[0];
                this.PayloadType = type.BaseType.GenericTypeArguments[1];
                break;
            case "BaseStep`1":
                this.DataType = type.BaseType.GenericTypeArguments[0];
                break;
        }
    }

    public bool HasData => this.dataProperty is not null;

    public bool HasPayload => this.payloadProperty is not null;

    public Type? DataType { get; }

    public Type? PayloadType { get; }

    public void SetData<TWorkflow, TWorkflowStep>(BaseStep<TWorkflow, TWorkflowStep> step, object? formData)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        this.dataProperty!.SetValue(step, formData);
    }

    public void SetPayload<TWorkflow, TWorkflowStep>(BaseStep<TWorkflow, TWorkflowStep> step, object? payload)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        this.payloadProperty!.SetValue(step, payload);
    }

    public BaseStep<TWorkflow, TWorkflowStep> CreateStep<TWorkflow, TWorkflowStep>(IServiceProvider services)
        where TWorkflow : class, IWorkflow
        where TWorkflowStep : class, IWorkflowStep
    {
        return (BaseStep<TWorkflow, TWorkflowStep>)ActivatorUtilities.CreateInstance(services, this.type);
    }
}