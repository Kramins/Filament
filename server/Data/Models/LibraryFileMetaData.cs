

using System.Text.Json;

namespace filament.data.models;
public class LibraryFileMetaData : EntityBase
{

    public int LibraryFileId { get; set; }
    public LibraryFile LibraryFile { get; set; } = null!;
    public string Name { get; set; } = null!;
    public JsonDocument Value { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Source { get; set; } = null!;

}