using System.ComponentModel.DataAnnotations;

namespace Nuages.TaskQueue;

public class TaskQueueWorkerOptions
{
    public bool Enabled { get; set; } = true;
    
    public int MaxMessagesCount { get; set; } = 10;
    public int WaitDelayInMillisecondsWhenNoMessages { get; set; } = 1000;
    
    [Required] public string QueueName { get; set; } = null!;
}