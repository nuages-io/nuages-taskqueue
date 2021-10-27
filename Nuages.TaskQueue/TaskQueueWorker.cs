using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nuages.Queue;

namespace Nuages.TaskQueue;

[ExcludeFromCodeCoverage]
public class TaskQueueWorker<T> : QueueWorkerService<T> where T : IQueueService
{
    private readonly TaskQueueWorkerOptions _options;
    private ITaskRunner? _jobRunner;
        
    public TaskQueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorkerService<T>> logger, IOptions<TaskQueueWorkerOptions> options) : base(serviceProvider, logger)
    {
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = ServiceProvider.CreateScope();
            
        _jobRunner =
            scope.ServiceProvider
                .GetRequiredService<ITaskRunner>();
            
        var enable = _options.Enabled;
        if (!enable)
            return;

        QueueName = _options.QueueName;

        if (string.IsNullOrEmpty(QueueName))
            throw new NullReferenceException("QueueName must be provided");
        
        
        await base.ExecuteAsync(stoppingToken);
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        if (_jobRunner == null)
            throw new Exception("_jobRunner is null");
            
        var job = JsonSerializer.Deserialize<RunnableTaskDefinition>(msg.Body);
        if (job == null)
            throw new Exception("Can't derserialize Job Definition");

        await _jobRunner.ExecuteAsync(job.AssemblyQualifiedName, job.JsonPayload);
                
        return true;
        
    }
}