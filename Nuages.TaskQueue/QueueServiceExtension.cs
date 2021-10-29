using System.Text.Json;
using Nuages.Queue;

namespace Nuages.TaskQueue;

public static class QueueServiceExtension
{
    public static async Task AddTaskToQueueAsync<T, TD>(this IQueueService queueService, string name, TD data)
    {
        var t = new RunnableTaskDefinition
        {
            AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
            JsonPayload = JsonSerializer.Serialize(data)
        };
        
        var fullName = await queueService.GetQueueFullNameAsync(name);

        await queueService.PublishToQueueAsync(fullName!, t);
    }
    
    public static async Task<string?> PublishToQueueAsync(this IQueueService queueService, string queueFullName, object data)
    {
        return await queueService.PublishToQueueAsync(queueFullName, JsonSerializer.Serialize(data));
    }
}