using System.Runtime.Serialization;
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
        if (!Path.Exists(library.Location))
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

    public IQueryable<LibraryFile> GetFiles(int libraryId, string path)
    {
        return _dataContext.LibraryFiles.Where(x => x.LibraryId == libraryId && x.Path == path);
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

    public string GetFileLibraryRealativePath(Library library, string file)
    {
        var relativePath = Path.GetRelativePath(library.Location, file);
        var directoryName = Path.GetDirectoryName(relativePath) + Path.DirectorySeparatorChar;
        
        
        //if (!file.StartsWith(library.Location))
        //{
        //    _logger.LogTrace($"File {file} does not start with library location {library.Location}");
        //    return file;
        //}

        //return file.Substring(library.Location.Length);

        return directoryName;
    }

    public string GetFullFilePath(Library library, string file)
    {
        return library.Location + file;
    }

    internal void DeleteFile(Library library, LibraryFile libraryFile)
    {
        throw new NotImplementedException();
    }

    internal object GetLibraryFiles(int libraryId, string path)
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