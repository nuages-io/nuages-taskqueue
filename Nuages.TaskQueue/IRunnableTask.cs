namespace Nuages.TaskQueue;

public interface IRunnableTask
{
    Task ExecuteAsync(string jsonPayload);
}