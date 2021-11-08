namespace Nuages.TaskRunner
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class RunnableTaskDefinition
    {
        public string AssemblyQualifiedName { get; set; } = string.Empty;
        public object? Payload { get; set; }
        public string? UserId { get; set; }
    }
}
