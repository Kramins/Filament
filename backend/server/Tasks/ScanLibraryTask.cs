using filament.data;
namespace filament.tasks;
public class ScanLibraryTask
{
    private readonly FilamentDataContext _dataContext;
    private readonly ILogger<ScanLibraryTask> _logger;

    public ScanLibraryTask(FilamentDataContext filamentDataContext, ILogger<ScanLibraryTask> logger)
    {
        _dataContext = filamentDataContext;
        _logger = logger;
    }

    public void Scan(int libraryid)
    {
        _logger.LogInformation($"Scanning library {libraryid}");
    }

}