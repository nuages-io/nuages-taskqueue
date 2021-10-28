using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue;
using Nuages.Queue.ASQ;

namespace Nuages.TaskQueue.ASQ;

// ReSharper disable once InconsistentNaming
public static class TaskASQConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddASQTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        Action<QueueOptions>? configureQueues = null,
        // ReSharper disable once InconsistentNaming
        Action<ASQQueueClientOptions>? configureASQClient = null,
        Action<TaskQueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<TaskQueueWorkerOptions>(configuration.GetSection("TaskQueueWorker"));
        services.Configure<ASQQueueClientOptions>(configuration.GetSection("ASQ"));
        services.Configure<QueueOptions>(configuration.GetSection("Queues"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);

        if (configureASQClient != null)
            services.Configure(configureASQClient);
        
        if (configureQueues != null)
            services.Configure(configureQueues);
        
        services.PostConfigure<ASQQueueClientOptions>(options =>
        {
            var configErrors = ValidationErrors(options).ToArray();
            // ReSharper disable once InvertIf
            if (configErrors.Any())
            {
                var aggregateErrors = string.Join(",", configErrors);
                var count = configErrors.Length;
                var configType = options.GetType().Name;
                throw new ApplicationException(
                    $"Found {count} configuration error(s) in {configType}: {aggregateErrors}");
            }
        });
        
        services.PostConfigure<TaskQueueWorkerOptions>(options =>
        {
            var configErrors = ValidationErrors(options).ToArray();
            // ReSharper disable once InvertIf
            if (configErrors.Any())
            {
                var aggregateErrors = string.Join(",", configErrors);
                var count = configErrors.Length;
                var configType = options.GetType().Name;
                throw new ApplicationException(
                    $"Found {count} configuration error(s) in {configType}: {aggregateErrors}");
            }
        });

        return services.AddScoped<IASQQueueService, ASQQueueService>()
            .AddScoped<ITaskRunner, TaskRunner>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<TaskQueueWorker<IASQQueueService>>();
    }

    private static IEnumerable<string> ValidationErrors(object option)
    {
        var context = new ValidationContext(option, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(option, context, results, true);
        foreach (var validationResult in results) yield return validationResult.ErrorMessage ?? "?";
    }
}