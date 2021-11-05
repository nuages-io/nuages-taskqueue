using System.Threading.Tasks;

namespace Nuages.TaskRunner
{
    
    public interface ITaskRunnerService
    {
        Task ExecuteAsync(RunnableTaskDefinition taskDef);
    }
}
