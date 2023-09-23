using filament.data.models;
using filament.scheduler;
using filament.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace filament.api.v1;

[ApiController]
[Route("api/v1/library/{libraryId}/files")]
public class FilesController
{

    [HttpGet]
    public ApiResponse<IEnumerable<FileDto>> GetLibraryFiles(int libraryId, [FromQuery] string path)
    {
        return null;
    }

    [HttpGet("{id}")]
    public ApiResponse<FileDetailsDto> GetLibraryFile(int libraryId, int id)
    {
        return null;
    }
}