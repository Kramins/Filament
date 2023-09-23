namespace filament.data.models;


public class Library : EntityBase
{
    public required String Name { get; set; }
    public required String Location { get; set; }
    public required String Type { get; set; }
    public ICollection<LibraryFile> Files { get; set; } = null!;
}