using System.Threading.Tasks;

namespace Nuages.TaskRunner
{
    
    public interface ITaskRunnerService
    {
        Task<IRunnableTask> ExecuteAsync(RunnableTaskDefinition taskDef);
    }
}
