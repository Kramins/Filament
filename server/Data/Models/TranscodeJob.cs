namespace filament.data.models;

public class TranscodeJob : EntityBase
{
    public int LibraryFileId { get; set; }
    public int TranscodeProfileId { get; set; }
    public string Status { get; set; } = null!;
    public string OutputPath { get; set; } = null!;
}