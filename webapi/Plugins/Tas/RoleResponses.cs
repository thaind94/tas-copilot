// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class RoleModuleResponse : BaseResponse
{
    [JsonPropertyName("data")] public RoleModuleResponseData Data { get; set; } = new ();
}

public class RoleModuleResponseData
{
    [JsonPropertyName("roles")] public IEnumerable<ModuleRole> Roles { get; set; } = Enumerable.Empty<ModuleRole>();
}

public class RoleProjectResponse : BaseResponse
{
    [JsonPropertyName("data")] public RoleProjectResponseData Data { get; set; } = new ();
}

public class RoleProjectResponseData
{
    [JsonPropertyName("roles")] public IEnumerable<ProjectRole> Roles { get; set; } = Enumerable.Empty<ProjectRole>();

    [JsonPropertyName("defaultMemberRoleId")]
    public Guid DefaultMemberRoleId { get; set; }
}
