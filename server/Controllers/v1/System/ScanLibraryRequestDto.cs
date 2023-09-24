using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace filament.api.v1
{
    public class ScanLibraryRequestDto
    {
        [Required]
        [SwaggerSchema("Id of the library.")]
        public int Id { get; set; }
    }
}