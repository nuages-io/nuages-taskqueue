using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue.Queue;

namespace Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue;

public class SimpleQueueWorker : TaskQueueWorker<ISimpleQueueService>
{
    public SimpleQueueWorker(IServiceProvider serviceProvider, ILogger<SimpleQueueWorker> logger, IOptions<QueueWorkerOptions> queueWorkerOptions) : 
        base(serviceProvider, logger, queueWorkerOptions)
    {
       
    }

    protected override async Task<List<QueueMessage>> ReceiveMessageAsync(ISimpleQueueService queueService)
    {
        return await queueService.DequeueMessageAsync(QueueName!, MaxMessagesCount);
    }

    protected override async Task DeleteMessageAsync(ISimpleQueueService queueService, string id, string receiptHandle)
    {
        await queueService.DeleteMessageAsync(QueueName!, id, receiptHandle);
    }

}