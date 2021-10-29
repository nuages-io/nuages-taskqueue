using System.Text.Json;
using Nuages.Queue;

namespace Nuages.TaskQueue;

public static class QueueServiceExtension
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static async Task<string?> AddTaskToQueueAsync<T, TD>(this IQueueService queueService, string name, TD data)
    {
        var t = new RunnableTaskDefinition
        {
            AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
            JsonPayload = JsonSerializer.Serialize(data)
        };
        
        var fullName = await queueService.GetQueueFullNameAsync(name);

        return await queueService.PublishToQueueAsync(fullName!, t);
    }

    private static async Task<string?> PublishToQueueAsync(this IQueueService queueService, string queueFullName, object data)
    {
        return await queueService.PublishToQueueAsync(queueFullName, JsonSerializer.Serialize(data));
    }
}