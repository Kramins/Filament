namespace filament.data.models;

public class Library : EntityBase
{
    public String Name { get; set; }
    public String Location { get; set; }
    public String Type { get; set; }
    public ICollection<LibraryFile> Files { get; set; } = null!;
}