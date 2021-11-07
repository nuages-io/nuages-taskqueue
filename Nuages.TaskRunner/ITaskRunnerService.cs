using System.Threading.Tasks;

namespace Nuages.TaskRunner
{
    public interface ITaskRunnerService
    {
        Task<IRunnableTask> ExecuteAsync(RunnableTaskDefinition taskDef);
        Task<T> ExecuteAsync<T>(RunnableTaskDefinition taskDef) where T : IRunnableTask;
        Task<T> ExecuteAsync<T>(object data) where T : IRunnableTask;
        Task<T> ExecuteAsync<T,TD>(TD data) where T : IRunnableTask;
        
    }
}
