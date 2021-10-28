using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue.ASQ;

namespace Nuages.TaskQueue.ASQ;

// ReSharper disable once InconsistentNaming
public static class TaskASQConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddASQTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        Action<TaskQueueWorkerOptions>? configureWorker = null,
        Action<QueueClientOptions>? configureClient = null)
    {
        services.Configure<TaskQueueWorkerOptions>((IConfiguration)configuration.GetSection("QueueWorker"));
        services.Configure<QueueClientOptions>((IConfiguration)configuration.GetSection("ASQ"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);

        if (configureClient != null)
            services.Configure(configureClient);
        
        return services.AddScoped<IASQQueueService, ASQQueueService>()
            .AddScoped<ITaskRunner, TaskRunner>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<TaskQueueWorker<IASQQueueService>>();
    }
}