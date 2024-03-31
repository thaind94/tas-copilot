// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CopilotChat.WebApi.Models.Storage;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CopilotChat.WebApi.Storage;

public class MongoDbContext<T> : IStorageContext<T> where T : IStorageEntity
{
    internal readonly IMongoCollection<T> Collection;

    public MongoDbContext(string connectionString, string database, string collection)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
        clientSettings.LinqProvider = LinqProvider.V3;
        MongoClient client = new(clientSettings);
        Collection = client.GetDatabase(database).GetCollection<T>(collection);
    }

    public async Task<IEnumerable<T>> QueryEntitiesAsync(Expression<Func<T, bool>> predicate)
    {
        return await this.Collection.AsQueryable().Where(predicate).ToListAsync();
    }

    public async Task<T> ReadAsync(string entityId, string partitionKey)
    {
        return await this.Collection.Find(mc => mc.Id == entityId).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await this.Collection.InsertOneAsync(entity);
    }

    public async Task UpsertAsync(T entity)
    {
        await this.Collection.ReplaceOneAsync(mc => mc.Id == entity.Id, entity, new ReplaceOptions {IsUpsert = true});
    }

    public async Task DeleteAsync(T entity)
    {
        await this.Collection.DeleteOneAsync(mc => mc.Id == entity.Id);
    }
}

public class MongoDbChatCopilotChatMessageContext : MongoDbContext<CopilotChatMessage>,
    ICopilotChatMessageStorageContext
{
    public MongoDbChatCopilotChatMessageContext(string connectionString, string database, string collection) : base(
        connectionString, database, collection)
    {
    }

    public async Task<IEnumerable<CopilotChatMessage>> QueryEntitiesAsync(
        Expression<Func<CopilotChatMessage, bool>> predicate,
        int skip = 0, int count = -1)
    {
        if (count == -1)
        {
            return await this.Collection.AsQueryable().Where(predicate).Skip(skip).OrderByDescending(m => m.Timestamp).ToListAsync();
        }

        return await this.Collection.AsQueryable().Where(predicate).Skip(skip).OrderByDescending(m => m.Timestamp).Take(count).ToListAsync();
    }
}
