// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class ModuleRole
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("isModuleOwner"), JsonIgnore]
    public bool IsModuleOwner { get; set; }
}

public class ProjectRole
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("isProjectOwner"), JsonIgnore]
    public bool IsProjectOwner { get; set; }
}
