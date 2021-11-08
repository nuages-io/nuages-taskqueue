using Amazon.SQS;
using Nuages.TaskQueue.SQS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigureTaskQueue(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
    webApplicationBuilder.Services.AddSQSTaskQueueWorker(webApplicationBuilder.Configuration);
}

