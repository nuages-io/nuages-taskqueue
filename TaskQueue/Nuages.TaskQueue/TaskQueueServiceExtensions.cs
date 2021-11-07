using System.Text.Json;
using Nuages.Queue;
using Nuages.TaskRunner;
// ReSharper disable UnusedMember.Global

namespace Nuages.TaskQueue;

// ReSharper disable once UnusedType.Global
public static class TaskQueueServiceExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static async Task<string?> EnqueueTaskAsync(this IQueueService queueService, string name, RunnableTaskDefinition taskdef)
    {
        var fullName = await queueService.GetQueueFullNameAsync(name);

        return await queueService.EnqueueMessageAsync(fullName!, JsonSerializer.Serialize(taskdef));
    }
    
    public static async Task<string?> EnqueueTaskAsync<T>(this IQueueService queueService, string name, object taskData) where T : IRunnableTask
    {
        var taskDef = RunnableTaskDefinitionCreator<T>.Create(taskData);
        
        var fullName = await queueService.GetQueueFullNameAsync(name);

        return await queueService.EnqueueMessageAsync(fullName!, JsonSerializer.Serialize(taskDef));
    }
}