
using filament.data.models;
using filament.scheduler;
using filament.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace filament.api.v1;

[ApiController]
[Route("api/v1/library")]
public class LibraryController
{
    private readonly ILogger<LibraryController> _logger;
    private readonly LibraryService _libraryService;

    public LibraryController(LibraryService libraryService, ILogger<LibraryController> logger)
    {
        _libraryService = libraryService;
        _logger = logger;

    }


    [HttpGet]
    public ApiResponse<IEnumerable<BasicLibraryDto>> Get()
    {
        _logger.LogInformation("Getting all libraries");
        var libraries = _libraryService.GetAllWithBasic().Select(x => new BasicLibraryDto()
        {
            Id = x.Id,
            Name = x.Name,
            Created = x.EntityCreated,
            Type = x.Type
        });

        return new ApiResponse<IEnumerable<BasicLibraryDto>>(libraries.ToList());
    }

    /// <summary>
    /// 
    /// </summary>
    /// Sample request:
    ///
    ///     POST /api/v1/library
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1",
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <param name="library"></param>
    [HttpPost]
    public ApiResponse<AddLibraryResponseDto> Post([FromBody] AddLibraryRequestDto library)
    {
        var newLibrary = new Library()
        {
            Name = library.Name,
            Type = library.Type,
            Location = library.Location,
        };

        var id = _libraryService.Add(newLibrary);

        _logger.LogInformation($"Added new library with id {id}");

        return new ApiResponse<AddLibraryResponseDto>(new AddLibraryResponseDto()
        {
            Id = id
        });

    }

}
