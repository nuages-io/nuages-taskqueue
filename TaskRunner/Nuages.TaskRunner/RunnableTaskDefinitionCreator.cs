using System.Text.Json;

namespace Nuages.TaskRunner
{
    public static class RunnableTaskDefinitionCreator<T> where T :  IRunnableTask
    {
        public static RunnableTaskDefinition Create(object? data)
        {
            var taskData = new RunnableTaskDefinition
            {
                AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
                Payload = data == null ? null : JsonSerializer.Serialize(data)
            };

            return taskData;
        }
    }
}