using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.Queue.SQS;
using Nuages.TaskQueue;
using Nuages.TaskQueue.SQS;
using Nuages.TaskRunner;
using Nuages.TaskRunner.Tasks;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName!)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

const string name = "Worker";

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging => { logging.AddConsole(); })
    .ConfigureServices(services =>
        {
            services
                .AddSingleton(configuration)
                .AddSQSTaskQueue()
                .Configure<QueueWorkerOptions>(name,
                    configuration.GetSection("TaskQueueWorker"))
                .AddSingleton<IHostedService>(x =>
                    ActivatorUtilities.CreateInstance<TaskQueueWorker<ISQSQueueService>>(x, name));
            
            AddSQS(services);
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<ISQSQueueService>();
    var options = provider.GetRequiredService<IOptionsMonitor<QueueWorkerOptions>>().Get(name);
    
    var taskData =
        RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new OutputToConsoleTaskData { Message = "Started !!!!" });
    
    await queueService.EnqueueTaskAsync(options.QueueName, taskData);
}

// ReSharper disable once InconsistentNaming
void AddSQS(IServiceCollection services, bool useProfile = true)
{
    if (useProfile)
    {
        //By default, we use a SQS profile to get credentials https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
        services.AddDefaultAWSOptions(configuration.GetAWSOptions())
            .AddAWSService<IAmazonSQS>();
    }
    else
    {
        var section = configuration.GetSection("SQS");
        var accessKey = section["AccessKey"];
        var secretKey = section["SecretKey"];
        var region = section["Region"];

        var sqsClient = new AmazonSQSClient(new BasicAWSCredentials(accessKey, secretKey),
            RegionEndpoint.GetBySystemName(region));

        services.AddSingleton<IAmazonSQS>(sqsClient);
    }
}