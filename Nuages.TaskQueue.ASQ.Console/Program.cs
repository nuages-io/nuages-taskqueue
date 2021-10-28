
using Microsoft.Extensions.Options;
using Nuages.Queue.ASQ;
using Nuages.TaskQueue;
using Nuages.TaskQueue.ASQ;
using Nuages.TaskQueue.Tasks;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .ConfigureServices(services =>
        services
            .AddSingleton(configuration)
            .AddASQTaskQueueWorker(configuration)
    );

var host = hostBuilder.UseConsoleLifetime().Build();

//Test Message
await SendTestMessage(host.Services);

await host.RunAsync();

async Task SendTestMessage(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<IASQQueueService>();
    var options = provider.GetRequiredService<IOptions<TaskQueueWorkerOptions>>().Value;

    await queueService.AddToTaskQueueAsync<OutputToConsoleTask>(options.QueueName!, new { Message = "Started !!!!" });
}