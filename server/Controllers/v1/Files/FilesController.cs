using filament.services;
using Microsoft.AspNetCore.Mvc;

namespace filament.api.v1;

[ApiController]
[Route("api/v1/library/{libraryId}/files")]
public class FilesController
{
    private readonly LibraryService _libraryService;

    public FilesController(LibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    [HttpGet("-/{path}")]
    public ApiResponse<IEnumerable<FileDto>> GetLibraryFiles(int libraryId, string path = "/")
    {
        var libraryFiles = _libraryService.GetFiles(libraryId, path);
        var files = libraryFiles.Select(x => new FileDto
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Size = x.Size,
        }).ToList();
        return files;
    }

    [HttpGet("{id}")]
    public ApiResponse<FileDetailsDto> GetLibraryFile(int libraryId, int id)
    {
        return null;
    }
}