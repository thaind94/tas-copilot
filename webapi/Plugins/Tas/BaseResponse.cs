// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class BaseResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("errorCode")] public string ErrorCode { get; set; } = string.Empty;
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
}
