namespace Nuages.TaskRunner
{
    public static class RunnableTaskDefinitionCreator<T> where T :  IRunnableTask
    {
        public static RunnableTaskDefinition Create(object? data, string? userId = null)
        {
            var taskData = new RunnableTaskDefinition
            {
                AssemblyQualifiedName = typeof(T).AssemblyQualifiedName!,
                Payload = data ,
                UserId = userId
            };

            return taskData;
        }
    }
}