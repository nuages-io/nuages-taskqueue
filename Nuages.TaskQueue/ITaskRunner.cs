namespace Nuages.TaskQueue;

public interface ITaskRunner
{
    Task ExecuteAsync(string assemblyQualifiedName, string jsonPayload);
}