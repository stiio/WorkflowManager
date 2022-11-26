using System.Text.Json;
using Stio.WorkflowManager.DemoApi.Enums;
using Stio.WorkflowManager.DemoApi.Models;

namespace Stio.WorkflowManager.Test;

public static class Extensions
{
    public static string ToJson<T>(this T obj, JsonSerializerOptions jsonOptions)
        where T : class
    {
        return JsonSerializer.Serialize(obj, jsonOptions);
    }

    public static T? FromJson<T>(this string json, JsonSerializerOptions jsonOptions)
    {
        return JsonSerializer.Deserialize<T>(json, jsonOptions);
    }

    public static string ToRequestUri(this NextStepResponse nextStepResponse)
    {
        return nextStepResponse.Step
            .ToRequestUriTemplate()
            .Replace("{workflowId}", nextStepResponse.WorkflowId.ToString())
            .Replace("{relatedObjectId}", nextStepResponse.RelatedObjectId);
    }

    private static string ToRequestUriTemplate(this Step step)
    {
        return step switch
        {
            Step.FirstBlockQuestion1 => "api/workflows/{workflowId}/first_block/first_question",
            Step.FirstBlockQuestion2 => "api/workflows/{workflowId}/first_block/second_question",
            Step.FirstBlockQuestion3 => "api/workflows/{workflowId}/first_block/third_question",
            Step.SecondBlockQuestion1 => "api/workflows/{workflowId}/second_block/first_question",
            Step.SecondBlockQuestion2 => "api/workflows/{workflowId}/second_block/second_question",
            Step.SecondBlockQuestion3 => "api/workflows/{workflowId}/second_block/third_question",
            Step.SecondBlockQuestion4 => "api/workflows/{workflowId}/second_block/fourth_question?relatedObjectId={relatedObjectId}",
            Step.ThirdBlockQuestion1 => "api/workflows/{workflowId}/third_block/first_question",
            Step.ThirdBlockQuestion2 => "api/workflows/{workflowId}/third_block/second_question?relatedObjectId={relatedObjectId}",
            Step.Review => "api/workflows/{workflowId}/review_block/review",
            Step.Completed => "api/workflows/{workflowId}/review_block/completed",
            _ => throw new ArgumentOutOfRangeException(nameof(step)),
        };
    }
}