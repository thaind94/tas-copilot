// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace CopilotChat.WebApi.Plugins.Tas;

public class ListUserResponse
{
    [JsonPropertyName("items")] public IEnumerable<User> Items { get; set; } = Enumerable.Empty<User>();
}

public class User
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
    [JsonPropertyName("userName")] public string UserName { get; set; } = string.Empty;
    [JsonPropertyName("firstName")] public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("lastName")] public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
    [JsonPropertyName("avatar")] public string Avatar { get; set; } = string.Empty;
}
