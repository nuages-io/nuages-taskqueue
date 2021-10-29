using Microsoft.Extensions.Logging;
using Nuages.Queue;
using Nuages.Queue.Simple;

namespace Nuages.SimpleQueue;

public class SimpleQueueWorker : QueueWorker<ISimpleQueueService>
{
    public SimpleQueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<ISimpleQueueService>> logger) : base(serviceProvider, logger)
    {
    }

    protected override Task<List<QueueMessage>> ReceiveMessageAsync(ISimpleQueueService queueService)
    {
        throw new NotImplementedException();
    }

    protected override Task DeleteMessageAsync(ISimpleQueueService queueService, string id, string receiptHandle)
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        throw new NotImplementedException();
    }
}