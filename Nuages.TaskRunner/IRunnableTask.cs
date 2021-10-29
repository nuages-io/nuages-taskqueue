namespace Nuages.TaskRunner;

public interface IRunnableTask
{
    Task ExecuteAsync(string jsonPayload);
}