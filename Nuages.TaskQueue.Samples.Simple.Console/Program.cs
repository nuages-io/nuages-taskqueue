﻿using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.TaskQueue;
using Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue;
using Nuages.TaskQueue.Samples.Simple.Console.SimpleQueue.Queue;
using Nuages.TaskRunner;
using Nuages.TaskRunner.Tasks;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName!)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging => { logging.AddConsole(); })
    .ConfigureServices(services =>
        {
            services
                .AddSingleton(configuration)
                .AddSimpleTaskQueueWorker(configuration);

            //Use this to add additional workers. It may be for the same queue or for another queue
            //.AddSingleton<IHostedService>(sp =>
            //   TaskQueueWorker<ISQSQueueService>.Create(sp, "TaskQueueTest-2")); 

            //Connection string is provided by IQueueClientProvider.
            //Provide another service implementation for IQueueClientProvider if you want to control the connection string by queue.
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<ISimpleQueueService>();
    var options = provider.GetRequiredService<IOptionsMonitor<QueueWorkerOptions>>().Get("TaskWorker");
    var taskData =
        RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new OutputToConsoleTaskData { Message = "Started !!!!" });
    await queueService.EnqueueTaskAsync(options.QueueName, taskData);
}
