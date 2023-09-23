

namespace filament.data.models;
public class LibraryFile : EntityBase
{
    public int LibraryId { get; set; }
    public Library Library { get; set; } = null!;
    public bool IsDirectory { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; }
    public long Size { get; set; } = 0;
    public string? Extension { get; set; } = null!;
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public ICollection<LibraryFileMetaData> MetaData { get; set; } = null!;
}