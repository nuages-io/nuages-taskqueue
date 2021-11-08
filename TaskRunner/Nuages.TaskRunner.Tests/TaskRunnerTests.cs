using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moq;
using Nuages.TaskRunner.Tasks;
using Xunit;

namespace Nuages.TaskRunner.Tests;

[ExcludeFromCodeCoverage]
public class TaskRunnerTests
{
    [Fact]
    public async Task ShouldExecuteAsync()
    {
        var taskData = new OutputToConsoleTaskData();
        var taskDef = RunnableTaskDefinitionCreator<OutputToConsoleTask>.Create(taskData);
        
        var serviceprovider = new Mock<IServiceProvider>();

        var runner = new TaskRunnerService(serviceprovider.Object, new List<ITaskAuthorizationService>());

        await runner.ExecuteAsync(taskDef);

    }
    
    // [Fact]
    // public async Task ShouldFailedExecuteAsync()
    // {
    //     const string name = "BadClassName";
    //     
    //     var taskData = new OutputToConsoleTaskData();
    //     var serviceprovider = new Mock<IServiceProvider>();
    //
    //     var runner = new TaskRunnerService(serviceprovider.Object);
    //
    //     await Assert.ThrowsAsync<Exception>(async () =>
    //     {
    //         await runner.ExecuteAsync(name, JsonSerializer.Serialize(taskData));
    //     });
    //    
    //
    // }
}