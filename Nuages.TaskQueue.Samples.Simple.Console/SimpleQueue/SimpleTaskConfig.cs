using System.ComponentModel.DataAnnotations;
using Nuages.Queue;
using Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue.Queue;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue;

// ReSharper disable once InconsistentNaming
public static class SimpleTaskConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddSimpleTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        // ReSharper disable once InconsistentNaming
        Action<QueueWorkerOptions>? configureWorker = null)
    {
        var name = "TaskWorker";
        
        services.Configure<QueueWorkerOptions>(name, configuration.GetSection("TaskQueueWorker"));
        
        if (configureWorker != null)
            services.Configure(name, configureWorker);

        return services.AddSingleton<ISimpleQueueService, SimpleQueueService>()
            .AddScoped<ITaskRunnerService, TaskRunnerService>()
            .AddSingleton<IHostedService>(x =>ActivatorUtilities.CreateInstance<SimpleQueueWorker>(x, name));
    }

}