using System.Runtime.Serialization;
using filament.data;
using filament.data.models;
using filament.scheduler;
using filament.tasks;

namespace filament.services;

public class TaskService
{

    FilamentDataContext _dataContext;

    private readonly ILogger<TaskService> _logger;
    private readonly SchedulerClientService _schedulerClientService;

    public TaskService(FilamentDataContext filamentDataContext, SchedulerClientService schedulerClientService, ILogger<TaskService> logger)
    {
        _dataContext = filamentDataContext;
        _logger = logger;
        _schedulerClientService = schedulerClientService;
    }

    public void ScanLibrary(int libraryId)
    {
        _schedulerClientService.PublishChannelTask<ScanLibraryTask>(x => x.Scan(libraryId), "background-tasks");
    }
}
