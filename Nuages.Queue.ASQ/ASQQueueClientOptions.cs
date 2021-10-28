using System.ComponentModel.DataAnnotations;

namespace Nuages.Queue.ASQ;

// ReSharper disable once InconsistentNaming
public class ASQQueueClientOptions
{
    [Required] public string ConnectionString { get; set; } = null!;
}