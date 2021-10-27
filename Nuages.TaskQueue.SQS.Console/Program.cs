
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Nuages.Queue.SQS;
using Nuages.TaskQueue;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

var section = configuration.GetSection("SQS");

var accessKey = section["AccessKey"];
var secretKey = section["SecretKey"];
var region = section["Region"];
var name = section["QueueName"];

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
            .AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<ITaskRunner, TaskRunner>()
            .AddSingleton<IAmazonSQS>(sqsCLient)
            .AddHostedService<TaskQueueWorker<ISQSQueueService>>()
            .Configure<TaskQueueWorkerOptions>(config =>
            {
                config.QueueName = name;
            })
        );


await hostBuilder.RunConsoleAsync();
