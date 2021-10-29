namespace Nuages.Queue;

public interface IQueueService
{
    Task<string?> GetQueueFullNameAsync(string queueName);
    // ReSharper disable once UnusedMember.Global
    Task<string?> PublishToQueueAsync(string queueFullName, string text);
    Task<List<QueueMessage>> ReceiveMessageAsync(string queueFullName, int maxMessages = 1);

    Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle);
}