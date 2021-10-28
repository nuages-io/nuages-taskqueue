using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Nuages.Queue;

    [ExcludeFromCodeCoverage]
  
    // ReSharper disable once UnusedType.Global
    public abstract class QueueWorker<T> : BackgroundService where T : IQueueService
    {
        protected string? QueueName { get; set; }
        protected int MaxMessagesCount { get; set; } = 10;
        protected int WaitDelayInMillisecondsWhenNoMessages { get; set; } = 1000;
        
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger<QueueWorker<T>> Logger;

        protected QueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<T>> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(QueueName))
                throw new Exception("QueueName must be provided");
            
            using var scope = ServiceProvider.CreateScope();

            var queueService = scope.ServiceProvider.GetRequiredService<T>();
            
            var queueFullName = await queueService.GetQueueFullNameAsync(QueueName);
            if (string.IsNullOrEmpty(queueFullName))
                throw new Exception($"Queue Url not found for {QueueName}");
           
            LogInformation($"Starting polling queue : {QueueName}");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await queueService.ReceiveMessageAsync(queueFullName, MaxMessagesCount);

                    if (messages.Any())
                    {
                        LogInformation($"{messages.Count} messages received {queueFullName}");

                        foreach (var msg in messages)
                        {
                            var result = await ProcessMessageAsync(msg);

                            if (result)
                            {
                                LogInformation($"{msg.MessageId} processed with success {queueFullName}");
                                await queueService.DeleteMessageAsync( queueFullName, msg.MessageId, msg.Handle);
                            }
                        }
                    }
                    else
                    {
                        LogInformation($"0 messages received {queueFullName}");
                        await Task.Delay(TimeSpan.FromMilliseconds(WaitDelayInMillisecondsWhenNoMessages), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                
            }
        }

        protected abstract Task<bool> ProcessMessageAsync(QueueMessage msg);

        protected virtual void LogInformation(string message)
        {
            Logger.LogInformation($"{message}");
        }
        
        protected virtual void LogError(string message)
        {
            Logger.LogError($"{message}");
        }
    }