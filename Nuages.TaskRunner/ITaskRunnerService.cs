namespace Nuages.TaskRunner;

public interface ITaskRunnerService
{
    Task ExecuteAsync(string assemblyQualifiedName, string payload);
}