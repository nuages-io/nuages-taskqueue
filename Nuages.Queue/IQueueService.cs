namespace Nuages.Queue;

public interface IQueueService
{
    Task<string?> GetQueueFullNameAsync(string queueName);
    // ReSharper disable once UnusedMember.Global
    Task<bool> PublishToQueueAsync(string queueFullName, string json);
    Task<bool> PublishToQueueAsync(string queueFullName, object data);
    Task<List<QueueMessage>> ReceiveMessageAsync(string queueFullName, int maxMessages = 1);

    Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle);
}