using System.ComponentModel.DataAnnotations;

namespace Nuages.TaskQueue;

public class TaskQueueWorkerOptions
{
    public bool Enabled { get; set; } = true;
    public bool CreateMissingQueue { get; set; } = true;
    [Required] public string QueueName { get; set; } = null!;
}