// Copyright (c) Microsoft. All rights reserved.

namespace CopilotChat.WebApi.Options;

public class MongoDbOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string ChatSessionsCollection { get; set; } = string.Empty;
    public string ChatMessagesCollection { get; set; } = string.Empty;
    public string ChatMemorySourcesCollection { get; set; } = string.Empty;
    public string ChatParticipantsCollection { get; set; } = string.Empty;
}
