
using Microsoft.Extensions.Options;
using Nuages.Queue.ASQ;
using Nuages.TaskQueue;
using Nuages.TaskQueue.ASQ;
using Nuages.TaskRunner.Tasks;

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
        {
            services
                .AddSingleton(configuration)
                .AddASQTaskQueueWorker(configuration); //This will create the first worker based on the appSettings.json config

            //Use this to add additional workers. It may be for the same queue or for another queue
            //.AddSingleton<IHostedService>(sp =>
            //   TaskQueueWorker<IASQQueueService>.Create(sp, "test-queue-2")); 

            //Connection string is provided by IQueueClientProvider.
            //Provide another service implementation for IQueueClientProvider if you want to control the connection string by queue.
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

//Test Message
await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<IASQQueueService>();
    var options = provider.GetRequiredService<IOptions<TaskQueueWorkerOptions>>().Value;

    await queueService.AddTaskToQueueAsync<OutputToConsoleTask, OutputToConsoleTaskData>(options.QueueName, new OutputToConsoleTaskData { Message = "Started !!!!" });
}

