using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

// ReSharper disable once InconsistentNaming
public class ASQQueueService : IASQQueueService
{
    private readonly IQueueClientProvider _clientProvider;
    private readonly QueueOptions _queryOptions;

    public ASQQueueService(IQueueClientProvider clientProvider, IOptions<QueueOptions> queryOptions)
    {
        _clientProvider = clientProvider;
        _queryOptions = queryOptions.Value;
    }
    
    public async Task<string?> GetQueueFullNameAsync(string queueName)
    {
        return await Task.FromResult(queueName);
    }

    public async Task<bool> PublishToQueueAsync(string fullQueueName, string data)
    {
        var client = _clientProvider.GetClient(fullQueueName);

        // Create the queue if it doesn't already exist
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();

        await client.SendMessageAsync(data);
        return true;
    }
    
    public async Task<bool> PublishToQueueAsync(string queueFullName, object data)
    {
        return await PublishToQueueAsync(queueFullName, JsonSerializer.Serialize(data));
    }

    public async Task<List<QueueMessage>> ReceiveMessageAsync(string fullQueueName, int maxMessages = 1)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();
        
        var messages = await client.ReceiveMessagesAsync(maxMessages);

        return messages.Value.Select(message => new QueueMessage { MessageId = message.MessageId, Body = message.MessageText, Handle = message.PopReceipt }).ToList();
    }

    public async Task DeleteMessageAsync(string fullQueueName, string id, string receiptHandle)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();
        
        await client.DeleteMessageAsync(id, receiptHandle);
    }
}