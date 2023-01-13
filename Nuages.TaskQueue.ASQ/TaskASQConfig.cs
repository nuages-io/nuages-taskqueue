using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nuages.Queue;
using Nuages.Queue.ASQ;
using Nuages.TaskRunner;

namespace Nuages.TaskQueue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedMember.Global
// ReSharper disable once UnusedType.Global
public static class TaskASQConfig
{
    public static IServiceCollection AddASQTaskQueue(this IServiceCollection services)
    {
        return services.AddScoped<IASQQueueService, ASQQueueService>()
            .AddScoped<ITaskRunnerService, TaskRunnerService>()
            .AddScoped<IASQQueueClientProvider, ASQQueueClientProvider>();
    }
}