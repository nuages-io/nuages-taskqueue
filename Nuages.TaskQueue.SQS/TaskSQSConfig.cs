using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue.SQS;

namespace Nuages.TaskQueue.SQS;

// ReSharper disable once InconsistentNaming
public static class TaskASQSConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddSQSTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        Action<TaskQueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<TaskQueueWorkerOptions>(configuration.GetSection("QueueWorker"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);

        return services.AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<ITaskRunner, TaskRunner>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<TaskQueueWorker<ISQSQueueService>>();
    }
}