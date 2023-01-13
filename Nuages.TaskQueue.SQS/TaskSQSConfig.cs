using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nuages.Queue;
using Nuages.Queue.SQS;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue.SQS;

// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class TaskASQSConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddSQSTaskQueue(this IServiceCollection services)
    {
        
        return services.AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<ITaskRunnerService, TaskRunnerService>()
            .AddScoped<ISQSQueueClientProvider, SQSQueueClientProvider>();
    }
}