# Nuages.TaskRunner

### What is Nuages.TaskRunner?

Nuages.TaskRunner is a .NET Core (3.1+) package that provides functionalities to execute code based on a generic serialized Json representation.

We call this a RunnableTask.

RunnableTask are run in the context of a TaskRunnerService. The TaskRunnerService can be hosted in a Web project, a console or any type of server side project.



## Sample

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

var taskData = RunnableTask<OutputToConsoleTask>.Create(data);
```

Finally, we get a TaskRunnerService instance, usually from the DI container. You can create in "manually" but you will have to provide a Service Provider instance to the constructor.

```csharp
//Here we instantiate using DI
MyConstructor(ITaskRunnerService taskRunnerService)
{
	_taskRunnerService = taskRunnerService;
}
```
Now we can execute in our method.

```csharp
var task = _TaskRunnerService.Execute(taskData);
```

## Additional Samples

You will find good examples on the usage in the following projects

- [Nuages.TaskQueue.SQS.Console](https://github.com/nuages-io/nuages-taskqueue-samples/tree/main/Nuages.TaskQueue.SQS.Console)
- [Nuages.TaskRunner.Samples.Lambda](https://github.com/nuages-io/nuages-taskqueue-samples/tree/main/Nuages.TaskRunner.Samples.Lambda)