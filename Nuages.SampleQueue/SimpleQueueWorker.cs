using Microsoft.Extensions.Logging;
using Nuages.Queue;
using Nuages.Queue.Simple;

namespace Nuages.SampleQueue;

public class SampleQueueWorker : QueueWorker<ISimpleQueueService>
{
    public SampleQueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<ISimpleQueueService>> logger) : base(serviceProvider, logger)
    {
    }

    protected override async Task<List<QueueMessage>> ReceiveMessageAsync(ISimpleQueueService queueService)
    {
        return await queueService.DequeueMessageAsync("", MaxMessagesCount);
    }

    protected override async Task DeleteMessageAsync(ISimpleQueueService queueService, string id, string receiptHandle)
    {
        await queueService.DeleteMessageAsync("", id, receiptHandle);
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        Logger.LogInformation("Message : {Message}", msg.Body);
        
        return await Task.FromResult(true);
    }
}