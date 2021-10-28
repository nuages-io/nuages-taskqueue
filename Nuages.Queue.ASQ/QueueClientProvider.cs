using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

// ReSharper disable once UnusedType.Global
public class QueueClientProvider : IQueueClientProvider
{
    private readonly QueueClientOptions _options;

    public QueueClientProvider(IOptions<QueueClientOptions> options)
    {
        _options = options.Value;
    }
    
    public QueueClient GetClient(string queueName)
    {
        return new QueueClient(_options.ConnectionString, queueName);
    }
}