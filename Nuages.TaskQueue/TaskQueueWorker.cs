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
    private readonly QueueWorkerOptions _options;
    private ITaskRunnerService? _taskRunner;
        
    // ReSharper disable once MemberCanBePrivate.Global
    public TaskQueueWorker(IServiceProvider serviceProvider, ILogger<TaskQueueWorker<T>> logger, 
                            IOptions<QueueWorkerOptions> options) : base(serviceProvider, logger)
    {
        _options = options.Value;
    }
   
    // ReSharper disable once UnusedMember.Global
    public static  TaskQueueWorker<T> Create(IServiceProvider sp, string queueName)
    {
        return new TaskQueueWorker<T>(
            sp,
            sp.GetRequiredService<ILogger<TaskQueueWorker<T>>>(),
            Options.Create(new QueueWorkerOptions
            {
                QueueName = queueName,
                Enabled = true
            }));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = ServiceProvider.CreateScope();
            
        _taskRunner =
            scope.ServiceProvider
                .GetRequiredService<ITaskRunnerService>();
            
        var enable = _options.Enabled;
        if (!enable)
            return;

        QueueName = _options.QueueName;
        MaxMessagesCount = _options.MaxMessagesCount;
        WaitDelayInMillisecondsWhenNoMessages = _options.WaitDelayInMillisecondsWhenNoMessages;
        
        await base.ExecuteAsync(stoppingToken);
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