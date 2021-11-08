using System.Text.Json;

namespace Nuages.TaskRunner
{
    public static class RunnableTaskDefinitionCreator<T> where T :  IRunnableTask
    {
        public static RunnableTaskDefinition Create(object? data, string? userId = null)
        {
            var taskData = new RunnableTaskDefinition
            {
                AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
                Payload = data == null ? null : JsonSerializer.Serialize(data),
                UserId = userId
            };

            return taskData;
        }
    }
}