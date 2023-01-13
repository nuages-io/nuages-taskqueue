
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.Queue.ASQ;
using Nuages.TaskQueue;
using Nuages.TaskQueue.ASQ;
using Nuages.TaskRunner;
using Nuages.TaskRunner.Tasks;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName!)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

const string name = "TaskWorker";

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .ConfigureServices(services =>
        {
            services
                .AddSingleton(configuration)
                .AddASQTaskQueue() //This will create the first worker based on the appSettings.json config
                .Configure<ASQQueueClientOptions>(configuration.GetSection("ASQ"))
                .Configure<QueueWorkerOptions>(name, configuration.GetSection("TaskQueueWorker"))
                .AddSingleton<IHostedService>(x =>
                    ActivatorUtilities.CreateInstance<TaskQueueWorker<IASQQueueService>>(x, name));
            
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

//Test Message
await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<IASQQueueService>();
    var options = provider.GetRequiredService<IOptionsMonitor<QueueWorkerOptions>>().Get(name);

    var taskData =
        RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new OutputToConsoleTaskData { Message = "Started !!!!" });

    await queueService.EnqueueTaskAsync(options.QueueName, taskData);
}

