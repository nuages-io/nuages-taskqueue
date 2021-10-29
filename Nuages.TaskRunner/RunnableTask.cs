using System.Text.Json;

namespace Nuages.TaskRunner;

public abstract class RunnableTask<T> : IRunnableTask
{
    // ReSharper disable once MemberCanBeProtected.Global
    public abstract Task ExecuteAsync(T? data);

    public virtual async Task ExecuteAsync(string jsonPayload)
    {
        if (!string.IsNullOrEmpty(jsonPayload))
        {
            var data = JsonSerializer.Deserialize<T>(jsonPayload);
            await ExecuteAsync(data);
        }
        else
        {
            await ExecuteAsync((T?) default);
        }
    }
}