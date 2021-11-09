using System.Text.Json;
using System.Threading.Tasks;

namespace Nuages.TaskRunner
{
    public abstract class RunnableTask<T> : IRunnableTask 
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public abstract Task ExecuteAsync(T data);

        public virtual async Task ExecuteAsync(string? payload)
        {
            if (!string.IsNullOrEmpty(payload))
            {
                var data = JsonSerializer.Deserialize<T>(payload);
                await ExecuteAsync(data!);
            }
        }
    }
}
