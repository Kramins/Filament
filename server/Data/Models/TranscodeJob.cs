namespace filament.data.models;

public class TranscodeJob : EntityBase
{
    public LibraryFile LibraryFile { get; set; } = null!;
    public int LibraryFileId { get; set; }
    public string OutputPath { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int TranscodeProfileId { get; set; }
}