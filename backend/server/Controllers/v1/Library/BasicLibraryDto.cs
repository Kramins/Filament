namespace filament.api.v1;

public class BasicLibraryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public DateTime Created { get; set; }

}