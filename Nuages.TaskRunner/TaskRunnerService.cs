using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nuages.TaskRunner
{
    
// ReSharper disable once UnusedType.Global
    public class TaskRunnerService : ITaskRunnerService
    {
        private readonly IServiceProvider _serviceProvider;
        
        public TaskRunnerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<IRunnableTask> ExecuteAsync(RunnableTaskDefinition taskDef)
        {
            var type = Type.GetType(taskDef.AssemblyQualifiedName);
            if (type == null)
            {
                throw new Exception(
                    $"Can't process task, type not found : {taskDef.AssemblyQualifiedName}");
            }
            
            var job = (IRunnableTask) ActivatorUtilities.CreateInstance(_serviceProvider, type);

            await job.ExecuteAsync(taskDef.Payload);
            
            return job;
        }
    }
}