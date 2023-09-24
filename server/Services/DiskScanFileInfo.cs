namespace filament.services;

public class DiskScanFileInfo
{
    public string Name { get; internal set; }
    public string? Path { get; internal set; }
    public long Size { get; internal set; }
    public string Extension { get; internal set; }
    public DateTime LastModified { get; internal set; }
}