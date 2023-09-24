using System.Text.Json;

namespace filament.data.models;

public class QueueItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public JsonDocument Payload { get; set; } = null!;
    public string Channel { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Error { get; set; }
    public DateTime? Started { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}