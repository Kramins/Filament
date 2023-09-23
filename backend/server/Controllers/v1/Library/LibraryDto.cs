

namespace filament.api.v1;

public class LibraryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public string Location { get; set; }
}