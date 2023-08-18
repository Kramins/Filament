using filament.data;
using filament.data.models;
using filament.scheduler;
using filament.tasks;

namespace filament.services;

public class LibraryService
{

    FilamentDataContext _dataContext;
    private readonly ILogger<LibraryService> _logger;
    private readonly SchedulerClientService _schedulerClientService;

    public LibraryService(FilamentDataContext filamentDataContext, SchedulerClientService schedulerClientService, ILogger<LibraryService> logger)
    {
        _dataContext = filamentDataContext;
        _logger = logger;
        _schedulerClientService = schedulerClientService;
    }

    public IEnumerable<Library> GetAllWithBasic()
    {
        return _dataContext.Libraries.AsQueryable();
    }

    public int Add(Library library)
    {
        _dataContext.Libraries.Add(library);
        _dataContext.SaveChanges();

        var libraryId = library.Id;
        _schedulerClientService.PublishBackgroundTask<ScanLibraryTask>(x => x.Scan(library.Id));

        return library.Id;
    }
}