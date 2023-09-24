using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace filament.api.v1;

public class AddLibraryRequestDto
{
    [Required]
    [SwaggerSchema("Name of the library.")]
    public string Name { get; set; }

    [Required]
    [SwaggerSchema("Type of the library.")]
    public string Type { get; set; }

    [Required]
    [SwaggerSchema("Location of the library.")]
    public string Location { get; set; }
}