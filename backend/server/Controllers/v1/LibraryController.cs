
using filament.dto.v1;
using Microsoft.AspNetCore.Mvc;

namespace filament.api.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class LibraryController
{
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(ILogger<LibraryController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<LibraryDto> Get()
    {
        return new List<LibraryDto>();
    }

    [HttpPost]
    public void Post([FromBody] AddLibraryDto library)
    {

    }

}
