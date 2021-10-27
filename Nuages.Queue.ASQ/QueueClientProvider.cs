using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

public class QueueClientProvider : IQueueClientProvider
{
    private readonly IOptionsMonitor<QueueClientOptions> _options;

    public QueueClientProvider(IOptionsMonitor<QueueClientOptions> options)
    {
        _options = options;
    }
    
    public QueueClient GetClient(string queueName)
    {
        return new QueueClient(_options.CurrentValue.ConnectionString, queueName);
    }
}