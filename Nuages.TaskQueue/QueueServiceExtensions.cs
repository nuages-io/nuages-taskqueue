using System.Text.Json;
using Nuages.Queue;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue;

public static class QueueServiceExtensions
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

        return await queueService.PublishToQueueAsync(fullName!, JsonSerializer.Serialize(data));
    }
}