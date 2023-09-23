using System.Runtime.Serialization;
using filament.data;
using filament.data.models;
using filament.scheduler;
using filament.tasks;

namespace filament.services;

public class LibraryService
{

    FilamentDataContext _dataContext;
    private readonly DiskScanService _diskScanService;
    private readonly ILogger<LibraryService> _logger;
    private readonly SchedulerClientService _schedulerClientService;

    public LibraryService(FilamentDataContext filamentDataContext, DiskScanService diskScanService, SchedulerClientService schedulerClientService, ILogger<LibraryService> logger)
    {
        _dataContext = filamentDataContext;
        _diskScanService = diskScanService;
        _logger = logger;
        _schedulerClientService = schedulerClientService;
    }

    public IEnumerable<Library> GetAllWithBasic()
    {
        return _dataContext.Libraries.AsQueryable();
    }

    public int Add(Library library)
    {
        if (!_diskScanService.Exists(library.Location))
        {
            _logger.LogError($"Library location {library.Location} does not exist");
            throw new FilamentException($"Library location {library.Location} does not exist");
        }

        _dataContext.Libraries.Add(library);
        _dataContext.SaveChanges();

        var libraryId = library.Id;
        _schedulerClientService.PublishChannelTask<ScanLibraryTask>(x => x.Scan(library.Id), "background-tasks");

        return library.Id;
    }

    public Library? GetLibrary(int libraryId)
    {
        return _dataContext.Libraries.FirstOrDefault(x => x.Id == libraryId);
    }
    public void Delete(int libraryId)
    {
        var library = _dataContext.Libraries.FirstOrDefault(x => x.Id == libraryId);
        if (library == null)
        {
            throw new FilamentException($"Library {libraryId} not found");
        }

        _dataContext.Libraries.Remove(library);
        _dataContext.SaveChanges();
    }

    public IQueryable<LibraryFile> GetFiles(int libraryId)
    {
        return _dataContext.LibraryFiles.Where(x => x.LibraryId == libraryId);
    }

    public void AddFile(Library library, LibraryFile libraryFile)
    {
        libraryFile.LibraryId = library.Id;
        _dataContext.LibraryFiles.Add(libraryFile);
        _dataContext.SaveChanges();
    }

    public void UpdateFile(Library library, LibraryFile directoryInfo)
    {
        _dataContext.LibraryFiles.Update(directoryInfo);
        _dataContext.SaveChanges();
    }

    public string TrimFilePath(Library library, string file)
    {
        if (!file.StartsWith(library.Location))
        {
            _logger.LogTrace($"File {file} does not start with library location {library.Location}");
            return file;
        }
        return file.Substring(library.Location.Length);
    }

    public string GetFullFilePath(Library library, string file)
    {
        return library.Location + file;
    }

    internal void DeleteFile(Library library, LibraryFile libraryFile)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
internal class FilamentException : Exception
{
    public FilamentException()
    {
    }

    public FilamentException(string? message) : base(message)
    {
    }

    public FilamentException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FilamentException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}