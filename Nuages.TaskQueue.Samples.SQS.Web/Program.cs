using Amazon.SQS;
using Nuages.Queue;
using Nuages.Queue.SQS;
using Nuages.TaskQueue;
using Nuages.TaskQueue.SQS;
// ReSharper disable UnusedParameter.Local

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

ConfigureTaskQueue(builder);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();

void ConfigureTaskQueue(WebApplicationBuilder webApplicationBuilder)
{
   
    
    webApplicationBuilder.Configuration.AddJsonFile("appsettings.local.json", true);
    webApplicationBuilder.Services.AddDefaultAWSOptions(webApplicationBuilder.Configuration.GetAWSOptions())
        .AddAWSService<IAmazonSQS>();
    webApplicationBuilder.Services.AddSQSTaskQueue();
    webApplicationBuilder.Services.Configure<QueueWorkerOptions>(Values.Name,
        webApplicationBuilder.Configuration.GetSection("TaskQueueWorker"));
    webApplicationBuilder.Services.AddSingleton<IHostedService>(x =>
        ActivatorUtilities.CreateInstance<TaskQueueWorker<ISQSQueueService>>(x, Values.Name));
}

public static class Values
{
    public const string Name = "Worker";
}

