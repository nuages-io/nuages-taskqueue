
# Nuages.TaskQueue

Nuages.TaskQueue bring Nuages.TaskRunner and Nuages.Queue together to offer a hosted task running service based on message from a queue.

It is available in two packages :

- Nuages.TaskQueue.SQS for AWS Simple Queue Service ([nuget](https://www.nuget.org/packages/Nuages.TaskQueue.SQS) | source)
- Nuages.TaskQueue.ASQ for Azure Storage Queue ([nuget](https://www.nuget.org/packages/Nuages.TaskQueue.ASQ) | source)

## How to use with SQS

### Configuration

```csharp

//Default SQS configuration
//You can provide your own implementation of IQueueClientProvider if you want to provide the instance using another way.
services.AddDefaultAWSOptions(configuration.GetAWSOptions())
        .AddAWSService<IAmazonSQS>();
        
services.AddSQSTaskQueueWorker(configuration);


```

appsettings.json

````json

{
  "Queues":
  {
    "AutoCreateQueue" : true
  },
  "TaskQueueWorker" :
  {
    "QueueName" : "here-you-enter-your-queuename",
    "Enabled" : true,
    "MaxMessagesCount" : 1,
    "WaitDelayInMillisecondsWhenNoMessages": 2000
  },
  "AWS": {
    "Profile": null,
    "Region": null
  }
}

````

Alternatively, you may want to provide the value using the Configure method.

```csharp

services.AddSQSTaskQueueWorker(configuration)
        .Configure<QueueOptions>(options =>
        {
            //set options here  
        }).Configure<QueueWorkerOptions>(options =>
        {
          //set options here  
        });
        
```

## Push message to the queue

You can push message on the queue the way you want. The important thing here is that the queue message body can be deserialize as a RunnableTaskDefinition.

This is what it should look like.

````json

{
  "AssemblyQualifiedName": "Nuages.TaskRunner.Tasks.OutputToConsoleTask, Nuages.TaskRunner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
  "Payload": "{\"Message\":\"Started !!!!\"}"
}

````


A service is provided to help you push such a message from your code. You need to use ISQSQueueService for that.

```csharp

var data = new OutputToConsoleTaskData { Message = message };
var taskData = RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(data);

//ISQSQueueService _isqsQueueService injected using DI
await _isqsQueueService.EnqueueTaskAsync("queue-name-goes-here", taskData);
        
```







## Samples

- Nuages.TaskQueue.Samples.SQS.Console
- Nuages.TaskQueue.Samples.SQS.Web
- Nuages.TaskQueue.Samples.ASQ.Console
- Nuages.TaskQueue.Samples.Simple.Console