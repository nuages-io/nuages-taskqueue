using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nuages.TaskRunner.Samples.Console
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }
        
        // ReSharper disable once UnusedParameter.Local
        private static async Task MainAsync(string[] args)
        {
            //Initialize teh DI container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ITaskRunnerService, TaskRunnerService>();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            //Read message from the console
            System.Console.WriteLine("What's your name?");
            var message = System.Console.ReadLine();
            
            //Get a task Runner
            var taskRunner = serviceProvider.GetRequiredService<ITaskRunnerService>();
            
            //Execute the task : Method 1 (Typed)
            var data = new OutputToConsoleTaskData( "Method 1 : Your name is : " + message);
            await taskRunner.ExecuteAsync<OutputToConsoleTask, OutputToConsoleTaskData>(data);
            
            //Execute the task : Method 2 (Data Typed)
            var taskDef =  RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(new { Message = "Method 2 : Your name is : " + message });
            await taskRunner.ExecuteAsync(taskDef);
            
            //Execute the task : Method 3 (Untyped), RunnableTaskDefinition can be serialized/deserialized
            var taskDef2 = new RunnableTaskDefinition
            {
                AssemblyQualifiedName = typeof(OutputToConsoleTask).AssemblyQualifiedName!,
                Payload = new { Message = "Method 3 : Your name is : " + message }
            };
            await taskRunner.ExecuteAsync(taskDef2);
            
        }
    }
    
    
    public class OutputToConsoleTask : RunnableTask<OutputToConsoleTaskData>  
    {  
        public override async Task ExecuteAsync(OutputToConsoleTaskData data)  
        {  
            await Task.Run(() => { System.Console.WriteLine(data.Message);});  
        }
    }  

    public class OutputToConsoleTaskData  
    {  
        // ReSharper disable once UnusedMember.Global
        public OutputToConsoleTaskData()  
        {  
        }  
        public OutputToConsoleTaskData(string message)  
        {  
            Message = message;  
        }
        public string Message { get; set; } = string.Empty;  
    }
}
