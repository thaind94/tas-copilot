// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class CreateModuleResponse : BaseResponse
{
    [JsonPropertyName("data")] public CreateModuleData Data { get; set; } = new();
}

public class CreateModuleData
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("createdBy")] public string CreatedBy { get; set; } = string.Empty;
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedBy")] public string UpdatedBy { get; set; } = string.Empty;
    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt { get; set; }
}
