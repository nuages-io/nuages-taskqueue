using Microsoft.Extensions.DependencyInjection;

namespace Nuages.TaskQueue;

// ReSharper disable once UnusedType.Global
public class TaskRunner : ITaskRunner
{
    private readonly IServiceProvider _serviceProvider;

    public TaskRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
        
    public async Task ExecuteAsync(string assemblyQualifiedName, string jsonPayload)
    {
        var type = Type.GetType(assemblyQualifiedName);
        if (type == null)
        {
            throw new Exception(
                $"Can't process task, type not found : {assemblyQualifiedName}");
        }
            
        var job = (IRunnableTask) ActivatorUtilities.CreateInstance(_serviceProvider, type);

        await job.ExecuteAsync(jsonPayload);
    }
        
}