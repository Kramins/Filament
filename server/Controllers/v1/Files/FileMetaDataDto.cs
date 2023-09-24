using System.Text.Json;

namespace filament.api.v1;

public class FileMetaDataDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public JsonDocument Value { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Source { get; set; } = null!;
}