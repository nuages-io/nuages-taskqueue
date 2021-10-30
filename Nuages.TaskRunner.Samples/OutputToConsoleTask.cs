

namespace Nuages.TaskRunner.Samples;

// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class OutputToConsoleTask : RunnableTask<OutputToConsoleTaskData>
{
    public override async Task ExecuteAsync(OutputToConsoleTaskData? data)
    {
        await Task.Run(() =>
        {
            if (data != null)
            {
                Console.WriteLine(data.Message);
            }
        });
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class OutputToConsoleTaskData
{
    public string Message { get; set; } = string.Empty;
}