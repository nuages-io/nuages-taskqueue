using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue;

[ExcludeFromCodeCoverage]
public class TaskQueueWorker<T> : QueueWorker<T> where T : IQueueService
{
    private ITaskRunnerService? _taskRunner;
        
    // ReSharper disable once MemberCanBePrivate.Global
    public TaskQueueWorker(string name, IServiceProvider serviceProvider, ILogger<TaskQueueWorker<T>> logger,  
        IOptionsMonitor<QueueWorkerOptions> options) : base(name, serviceProvider, logger, options)
    {
        var o = options.Get(name);
    }

    protected override void InitializeDependencies(IServiceScope scope)
    {
        _taskRunner =
            scope.ServiceProvider
                .GetRequiredService<ITaskRunnerService>();
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        if (_taskRunner == null)
            throw new Exception("_jobRunner is null");
            
        var task = JsonSerializer.Deserialize<RunnableTaskDefinition>(msg.Body);
        if (task == null)
            throw new Exception("Can't derserialize Job Definition");

        await _taskRunner.ExecuteAsync(task);
                
        return true;
    }
}