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
            using var scope = ServiceProvider.CreateScope();

            var queueService = scope.ServiceProvider.GetRequiredService<T>();

            await InitializeAsync(queueService);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await ReceiveMessageAsync(queueService);

                    if (messages.Any())
                    {
                        LogInformation($"{messages.Count} messages received");

                        foreach (var msg in messages)
                        {
                            var result = await ProcessMessageAsync(msg);

                            if (result)
                            {
                                LogInformation($"{msg.MessageId} processed with success ");
                                await DeleteMessageAsync(queueService, msg.MessageId, msg.Handle);
                            }
                        }
                    }
                    else
                    {
                        LogInformation("0 messages received");
                        await Task.Delay(TimeSpan.FromMilliseconds(WaitDelayInMillisecondsWhenNoMessages), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                
            }
        }

        protected abstract Task<List<QueueMessage>> ReceiveMessageAsync(T queueService);
        protected abstract Task DeleteMessageAsync(T queueService, string id, string receiptHandle);

        protected virtual async Task InitializeAsync(T queueService)
        {
           await  Task.FromResult(0);
        }
        
        protected abstract Task<bool> ProcessMessageAsync(QueueMessage msg);

        protected virtual void LogInformation(string message)
        {
            Logger.LogInformation("{message}", message);
        }
        
        protected virtual void LogError(string message)
        {
            Logger.LogError("{message}", message);
        }
    }