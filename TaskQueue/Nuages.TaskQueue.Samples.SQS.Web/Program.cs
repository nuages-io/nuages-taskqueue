using Amazon.SQS;
using Nuages.Queue;
using Nuages.Queue.SQS;
using Nuages.TaskQueue.SQS;
// ReSharper disable UnusedParameter.Local

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
    webApplicationBuilder.Services.AddSQSTaskQueueWorker(webApplicationBuilder.Configuration)
        .Configure<QueueOptions>(options =>
        {
            //set options here  
        }).Configure<QueueWorkerOptions>(options =>
        {
          //set options here  
        });
}

