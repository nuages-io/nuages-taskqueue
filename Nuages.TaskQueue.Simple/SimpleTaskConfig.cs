using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue.Simple;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue.Simple;

// ReSharper disable once InconsistentNaming
public static class SimpleTaskConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        // ReSharper disable once InconsistentNaming
        Action<TaskQueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<TaskQueueWorkerOptions>(configuration.GetSection("TaskQueueWorker"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);
        
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

        return services.AddScoped<ISimpleQueueService, SimpleQueueService>()
            .AddScoped<ITaskRunnerService, TaskRunnerService>()
            .AddHostedService<TaskQueueWorker<ISimpleQueueService>>();
    }

    private static IEnumerable<string> ValidationErrors(object option)
    {
        var context = new ValidationContext(option, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(option, context, results, true);
        foreach (var validationResult in results) yield return validationResult.ErrorMessage ?? "?";
    }
}