using System.Threading.Tasks;

namespace Nuages.TaskRunner
{
    public interface IRunnableTask
    {
        Task ExecuteAsync(string? payload);
    }
}