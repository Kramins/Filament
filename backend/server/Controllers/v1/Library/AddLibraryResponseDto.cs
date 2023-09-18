using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace filament.api.v1;


public class AddLibraryResponseDto
{
    [Required]
    [SwaggerSchema("Id of the library.")]
    public int Id { get; set; }
}