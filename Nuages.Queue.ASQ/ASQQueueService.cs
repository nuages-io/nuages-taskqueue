using System.Text.Json;

namespace Nuages.Queue.ASQ;

// ReSharper disable once InconsistentNaming
public class ASQQueueService : IASQQueueService
{
    private readonly IQueueClientProvider _clientProvider;

    public ASQQueueService(IQueueClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
    }
    
    public async Task<string?> GetQueueFullNameAsync(string queueName)
    {
        return await Task.FromResult(queueName);
    }

    public async Task<bool> PublishToQueueAsync(string fullQueueName, string data)
    {
        var client = _clientProvider.GetClient(fullQueueName);

        // Create the queue if it doesn't already exist
        await client.CreateIfNotExistsAsync();

        if (await client.ExistsAsync())
        {
            // Send a message to the queue
           await client.SendMessageAsync(data);
           return true;
        }

        return false;
    }
    
    public async Task<bool> PublishToQueueAsync(string queueFullName, object data)
    {
        return await PublishToQueueAsync(queueFullName, JsonSerializer.Serialize(data));
    }

    public async Task<List<QueueMessage>> ReceiveMessageAsync(string fullQueueName, int maxMessages = 1)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        await client.CreateIfNotExistsAsync();
        
        var messages = await client.ReceiveMessagesAsync(maxMessages);

        return messages.Value.Select(message => new QueueMessage { MessageId = message.MessageId, Body = message.MessageText, Handle = message.PopReceipt }).ToList();
    }

    public async Task DeleteMessageAsync(string fullQueueName, string id, string receiptHandle)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        await client.CreateIfNotExistsAsync();
        
        await client.DeleteMessageAsync(id, receiptHandle);
    }
}