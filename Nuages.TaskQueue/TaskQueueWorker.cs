using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nuages.Queue;

namespace Nuages.TaskQueue;

[ExcludeFromCodeCoverage]
public class TaskQueueWorker<T> : QueueWorker<T> where T : IQueueService
{
    private string? QueueName { get; set; }
    private string? QueueNameFullName { get; set; }
    
    private readonly TaskQueueWorkerOptions _options;
    private ITaskRunner? _jobRunner;
        
    public TaskQueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<T>> logger, 
                            IOptions<TaskQueueWorkerOptions> options) : base(serviceProvider, logger)
    {
        _options = options.Value;
    }

    protected override async Task InitializeAsync(T queueService)
    {
        QueueNameFullName = await queueService.GetQueueFullNameAsync(QueueName!);
        if (string.IsNullOrEmpty(QueueNameFullName))
            throw new Exception($"Queue Url not found for {QueueName}");
    }
    
    // ReSharper disable once UnusedMember.Global
    public static  TaskQueueWorker<T> Create(IServiceProvider sp, string queueName)
    {
        return new TaskQueueWorker<T>(
            sp,
            sp.GetRequiredService<ILogger<QueueWorker<T>>>(),
            Options.Create(new TaskQueueWorkerOptions
            {
                QueueName = queueName,
                Enabled = true
            }));
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
        MaxMessagesCount = _options.MaxMessagesCount;
        WaitDelayInMillisecondsWhenNoMessages = _options.WaitDelayInMillisecondsWhenNoMessages;

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

    protected override async Task<List<QueueMessage>> ReceiveMessageAsync(T queueService)
    {
        if (string.IsNullOrEmpty(QueueNameFullName))
            throw new NullReferenceException(QueueNameFullName);
        
        return await queueService.ReceiveMessageAsync(QueueNameFullName, MaxMessagesCount);
    }

    protected override async Task DeleteMessageAsync(T queueService, string id, string receiptHandle)
    {
        if (string.IsNullOrEmpty(QueueNameFullName))
            throw new NullReferenceException(QueueNameFullName);
        
        await queueService.DeleteMessageAsync(QueueNameFullName, id, receiptHandle);
    }

    protected override void LogInformation(string message)
    {
        Logger.LogInformation("{message} : {QueueNameFullName}",message, QueueNameFullName);
    }
    
    protected override void LogError(string message)
    {
        Logger.LogError("{message} : {QueueNameFullName}",message, QueueNameFullName);
    }
}