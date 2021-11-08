using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Nuages.TaskRunner
{
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class TaskRunnerService : ITaskRunnerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<ITaskAuthorizationService> _authorizers;

        public TaskRunnerService(IServiceProvider serviceProvider, IEnumerable<ITaskAuthorizationService> authorizers)
        {
            _serviceProvider = serviceProvider;
            _authorizers = authorizers;
        }

        public async Task<T> ExecuteAsync<T>(RunnableTaskDefinition taskDef) where T : IRunnableTask
        {
            return (T) await ExecuteAsync(taskDef);
        }
        
        public async Task<T> ExecuteAsync<T,TD>(TD data, string? userId = null) where T : IRunnableTask
        {
            var taskDef = RunnableTaskDefinitionCreator<T>.Create(data, userId);
            
            return (T) await ExecuteAsync(taskDef);
        }
        
        public async Task<T> ExecuteAsync<T>(object data, string? userId = null) where T : IRunnableTask
        {
            var taskDef = RunnableTaskDefinitionCreator<T>.Create(data, userId);
            
            return (T) await ExecuteAsync(taskDef);
        }

        protected virtual async Task<bool> IsAuthorizedToRunAsync(RunnableTaskDefinition taskDef)
        {
            if (!_authorizers.Any())
                return true;

            foreach (var authorizer in _authorizers)
            {
                if (!await authorizer.IsAuthorizedToRunAsync(taskDef))
                    return false;
            }

            return true;
        }
        
        public async Task<IRunnableTask> ExecuteAsync(RunnableTaskDefinition taskDef)
        {
            var type = Type.GetType(taskDef.AssemblyQualifiedName);
            if (type == null)
            {
                throw new Exception(
                    $"Can't process task, type not found : {taskDef.AssemblyQualifiedName}");
            }

            if (!await IsAuthorizedToRunAsync(taskDef))
                throw new Exception("NotAuthorized");
            
            var job = (IRunnableTask) ActivatorUtilities.CreateInstance(_serviceProvider, type);

            await job.ExecuteAsync(JsonSerializer.Serialize(taskDef.Payload));
            
            return job;
        }
    }
}