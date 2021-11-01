using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Nuages.TaskRunner;
using Nuages.TaskRunner.Tasks;
using Xunit;

namespace Nuages.Tests;

[ExcludeFromCodeCoverage]
public class TaskRunnerTests
{
    [Fact]
    public async Task ShouldExecuteAsync()
    {
        var name = typeof(OutputToConsoleTask).AssemblyQualifiedName;
        
        var taskData = new OutputToConsoleTaskData();
        var serviceprovider = new Mock<IServiceProvider>();

        var runner = new TaskRunnerService(serviceprovider.Object);

        await runner.ExecuteAsync(name!, JsonSerializer.Serialize(taskData));

    }
    
    [Fact]
    public async Task ShouldFailedExecuteAsync()
    {
        const string name = "BadClassName";
        
        var taskData = new OutputToConsoleTaskData();
        var serviceprovider = new Mock<IServiceProvider>();

        var runner = new TaskRunnerService(serviceprovider.Object);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await runner.ExecuteAsync(name, JsonSerializer.Serialize(taskData));
        });
       

    }
}