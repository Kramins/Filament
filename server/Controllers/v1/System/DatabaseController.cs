using filament.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace filament.api.v1.system;

[Route("api/v1/system/database")]
public class DatabaseController
{
    private readonly ILogger<DatabaseController> _logger;
    private readonly FilamentDataContext _dataContext;

    public DatabaseController(ILogger<DatabaseController> logger, FilamentDataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpPost("purge")]
    public void Purge()
    {
        _logger.LogInformation("Purging database");

        _dataContext.Database.EnsureDeleted();
        _dataContext.Database.Migrate();
    }
}