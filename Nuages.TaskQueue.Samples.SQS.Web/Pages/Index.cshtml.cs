using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.Queue.SQS;
using Nuages.TaskRunner;
using Nuages.TaskRunner.Tasks;
// ReSharper disable UnusedMember.Global

namespace Nuages.TaskQueue.Samples.SQS.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ISQSQueueService _isqsQueueService;
    private readonly QueueWorkerOptions _options;

    public IndexModel( ISQSQueueService isqsQueueService, IOptions<QueueWorkerOptions> options)
    {
        _isqsQueueService = isqsQueueService;
        _options = options.Value;
    }

    public async Task OnGet()
    {
        var taskData =
            RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new OutputToConsoleTaskData { Message = "Started !!!!" });
    
        await _isqsQueueService.EnqueueTaskAsync(_options.QueueName, taskData);

    }
    
    public async Task OnPost()
    {
        var message = Request.Form["message"];
        
        var taskData =
            RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new OutputToConsoleTaskData { Message = message });
    
        await _isqsQueueService.EnqueueTaskAsync(_options.QueueName, taskData);

    }
}