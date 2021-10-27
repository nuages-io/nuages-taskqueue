using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable MemberCanBePrivate.Global

namespace Nuages.Queue;

    [ExcludeFromCodeCoverage]
    [SuppressMessage("Usage", "CA2254", MessageId = "The logging message template should not vary between calls to \'LoggerExtensions.LogInformation(ILogger, string, params object[])\'")]
    // ReSharper disable once UnusedType.Global
    public abstract class QueueWorkerService<T> : BackgroundService where T : IQueueService
    {
        protected string? QueueName { get; set; }
        protected int MaxMessages { get; set; } = 10;
        protected int WaitDelayWhenNoMessages { get; set; } = 15;
        
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger<QueueWorkerService<T>> Logger;

        protected QueueWorkerService(IServiceProvider serviceProvider, ILogger<QueueWorkerService<T>> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }
        
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(QueueName))
                throw new Exception("QueueName must be provided");
            
            //The queue service is registered as scope because we might want to have more than one backgroud queue service in the same app.
            //Since the BackgroundWorkerService is not a scoped service, it will throw an exception if we try to inject a scoped service
            //We muste then create a new scope and use is to instantiuate the required service
            //You may register IQueueService as a singleton and inject it in the constructor if you like. Your choice.
            
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
                    var messages = await queueService.ReceiveMessageAsync(queueFullName, MaxMessages);

                    if (messages.Any())
                    {
                        LogInformation($"{messages.Count} messages received");

                        foreach (var msg in messages)
                        {
                            var result = await ProcessMessageAsync(msg);

                            if (result)
                            {
                                LogInformation($"{msg.MessageId} processed with success");
                                await queueService.DeleteMessageAsync( queueFullName, msg.MessageId, msg.Handle);
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(WaitDelayWhenNoMessages), stoppingToken);
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
            Logger.LogInformation(message);
        }
        
        protected virtual void LogError(string message)
        {
            Logger.LogError(message);
        }
    }