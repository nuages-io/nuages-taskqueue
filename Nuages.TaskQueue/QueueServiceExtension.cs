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
}