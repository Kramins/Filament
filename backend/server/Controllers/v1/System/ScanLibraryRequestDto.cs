using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace filament.api.v1
{
    public class ScanLibraryRequestDto
    {
        [Required]
        [SwaggerSchema("Id of the library.")]
        public int Id { get; set; }
    }
}