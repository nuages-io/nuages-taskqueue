namespace Nuages.TaskQueue;

public class TaskQueueWorkerOptions
{
    public bool Enabled { get; set; } = true;
    public string? QueueName { get; set; }
}