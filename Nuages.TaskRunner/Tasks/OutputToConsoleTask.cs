
using System;
using System.Threading.Tasks;

namespace Nuages.TaskRunner.Tasks
{

// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
    public class OutputToConsoleTask : RunnableTask<OutputToConsoleTaskData>
    {
        public override async Task ExecuteAsync(OutputToConsoleTaskData data)
        {
            await Task.Run(() =>
            {
                Console.WriteLine(data.Message);
            });
        }
    }

// ReSharper disable once ClassNeverInstantiated.Global
    public class OutputToConsoleTaskData
    {
        public OutputToConsoleTaskData()
        {
            
        }
        
        // ReSharper disable once UnusedMember.Global
        public OutputToConsoleTaskData(string message)
        {
            Message = message;
        }
        
        public string Message { get; set; } = string.Empty;
    }
}