# Nuages.TaskRunner

## What is Nuages.TaskRunner?

Nuages.TaskRunner is a .NET Core C# library that provides functionalities to execute code based on a task definition.

We call this a RunnableTask.

RunnableTask are run in the context of a TaskRunnerService. The TaskRunnerService can be hosted in a Web project, a console, serverless or any type of server side project.


## How does it works?

Let's start with a sample runnable class. This RunnableTask will output the content of a message to the Console.

```csharp
public class OutputToConsoleTask : RunnableTask<OutputToConsoleTaskData>  
{  
    public override async Task ExecuteAsync(OutputToConsoleTaskData data)  
    {  
        await Task.Run(() => { Console.WriteLine(data.Message);});  
    }
}  

public class OutputToConsoleTaskData  
{  
    public OutputToConsoleTaskData()  
    {  
    }  
    public OutputToConsoleTaskData(string message)  
    {  
        Message = message;  
    }
    public string Message { get; set; } = string.Empty;  
}
```

Now we need to create a RunnableTaskDefinition to pass as an input to the TaskRunnerService.

```csharp
var data = new OutputToConsoleTaskData("Hello!");

var taskData = RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(data);
```

Finally, we get a TaskRunnerService instance, usually from the DI container. You can create in "manually" but you will have to provide a Service Provider instance to the constructor.

```csharp
//Here we instantiate using DI
MyConstructor(ITaskRunnerService taskRunnerService)
{
    _taskRunnerService = taskRunnerService;
}
```
Now we can execute the task.

```csharp
var task = _TaskRunnerService.Execute(taskData);
```

## Sample : Using TaskRunner in the Console

This sample shows how to use TaskRunner with the Console. Not the typical usage but it shows how it works.

- [Nuages.TaskRunner.Samples.Console](https://github.com/nuages-io/)

## Sample : Using TaskRunner with AWS Lambda and SQS

TaskRunner can be used in a Lambda function to execute task based on message posted to an SQS queue.

- [Nuages.TaskRunner.Samples.Lambda](https://github.com/nuages-io/)

## Sample : Using TaskRunner with Nuages.TaskQueue and SQS

Don't want to use Lambda? You can host your own TaskQueue in your project. See the following sample.

- [Nuages.TaskQueue.SQS.Console](https://github.com/nuages-io/)
