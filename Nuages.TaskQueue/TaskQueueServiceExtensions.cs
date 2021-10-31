using System.Text.Json;
using System.Threading.Tasks;
using Nuages.Queue;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue;

public static class TaskQueueServiceExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static async Task<string?> EnqueueTaskAsync<T, TD>(this IQueueService queueService, string name, TD data)
    {
        var taskData = new RunnableTaskDefinition
        {
            AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(data)
        };
        
        var fullName = await queueService.GetQueueFullNameAsync(name);

        return await queueService.EnqueueMessageAsync(fullName!, JsonSerializer.Serialize(taskData));
    }
}