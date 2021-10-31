using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nuages.Queue;

public interface IQueueService
{
    Task<string?> GetQueueFullNameAsync(string queueName);
    // ReSharper disable once UnusedMember.Global
    Task<string?> EnqueueMessageAsync(string queueFullName, string text);
    Task<List<QueueMessage>> DequeueMessageAsync(string queueFullName, int maxMessages = 1);

    Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle);
}