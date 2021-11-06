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
    public TaskQueueWorker(IServiceProvider serviceProvider, ILogger<TaskQueueWorker<T>> logger,  IOptions<QueueWorkerOptions> options) : base(serviceProvider, logger, options)
    {
       
    }
   
    // ReSharper disable once UnusedMember.Global
    public static  TaskQueueWorker<T> Create(IServiceProvider sp, string queueName)
    {
        return new TaskQueueWorker<T>(
            sp,
            sp.GetRequiredService<ILogger<TaskQueueWorker<T>>>(),
            Microsoft.Extensions.Options.Options.Create(new QueueWorkerOptions
            {
                QueueName = queueName,
                Enabled = true
            }));
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