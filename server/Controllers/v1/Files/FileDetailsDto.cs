namespace filament.api.v1;

public class FileDetailsDto
{
    public int Id { get; set; }
    public bool IsDirectory { get; set; }
    public string Name { get; set; } = null!;
    public string? Path { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; } = null!;
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    public List<FileMetaDataDto> MetaData { get; set; } = null!;
}