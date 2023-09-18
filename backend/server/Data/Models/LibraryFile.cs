

namespace filament.data.models;
public class LibraryFile : EntityBase
{
    public int LibraryId { get; set; }
    public Library Library { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Path { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; } = null!;
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
}