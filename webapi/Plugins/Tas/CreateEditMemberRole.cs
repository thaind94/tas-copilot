// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class CreateEditMemberRole
{
    [JsonPropertyName("roleId")] public Guid RoleId { get; set; }
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
}
