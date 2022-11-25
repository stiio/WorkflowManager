﻿using System.Text.Json;

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
}