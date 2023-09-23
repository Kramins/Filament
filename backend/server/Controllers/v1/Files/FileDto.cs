namespace filament.api.v1;

public class FileDto
{
    public int Id { get; set; }
    public bool IsDirectory { get; set; }
    public string Name { get; set; } = null!;
    public string? Path { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; } = null!;
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }

}