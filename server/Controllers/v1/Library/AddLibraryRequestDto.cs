using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
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