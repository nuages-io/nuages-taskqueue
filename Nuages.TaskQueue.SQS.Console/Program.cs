using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;
using Nuages.TaskQueue;
using Nuages.TaskQueue.SQS;
using Nuages.TaskQueue.Tasks;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

var section = configuration.GetSection("SQS");
var accessKey = section["AccessKey"];
var secretKey = section["SecretKey"];
var region = section["Region"];

var sqsCLient = new AmazonSQSClient(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.GetBySystemName(region));

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .ConfigureServices(services =>
        services
            .AddLogging()
            .AddSingleton(configuration)
            .AddSingleton<IAmazonSQS>(sqsCLient)
            .AddSQSTaskQueueWorker(configuration)
        );

var host = hostBuilder.UseConsoleLifetime().Build();

await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<ISQSQueueService>();
    var options = provider.GetRequiredService<IOptions<TaskQueueWorkerOptions>>().Value;
    await queueService.AddToTaskQueueAsync<OutputToConsoleTask>(options.QueueName!, new { Message = "Started !!!!" });
}
