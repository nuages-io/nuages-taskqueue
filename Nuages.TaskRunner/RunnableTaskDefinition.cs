namespace Nuages.TaskRunner;

// ReSharper disable once ClassNeverInstantiated.Global
public class RunnableTaskDefinition
{
    public string AssemblyQualifiedName { get; set; } = string.Empty;
    public string JsonPayload { get; set; }= string.Empty;
}