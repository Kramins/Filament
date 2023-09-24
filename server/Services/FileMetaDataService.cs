using filament.data;
using filament.data.models;
using filament.providers.metadata;

namespace filament.services;

public class FileMetaDataService
{
    private readonly ILogger<FileMetaDataService> _logger;
    private readonly FilamentDataContext _filamentDataContext;
    private readonly LibraryService _libraryService;
    private ICollection<IFileMetaDataProvider> _metadataDetailsProviders;

    public FileMetaDataService(ILogger<FileMetaDataService> logger, FilamentDataContext filamentDataContext, LibraryService libraryService)
    {
        _logger = logger;
        _filamentDataContext = filamentDataContext;
        _libraryService = libraryService;
        _metadataDetailsProviders = new List<IFileMetaDataProvider>();

        _metadataDetailsProviders.Add(new FfmpegMetaDataProvider(logger));
    }

    public void ScanFile(Library library, LibraryFile file)
    {
        _logger.LogInformation($"Scanning file {file.Path}");
        var fullPath = _libraryService.GetFullFilePath(library, file.Path);
        var metadata = _metadataDetailsProviders.SelectMany(x => x.ScanAsync(fullPath).Result).ToList();

        metadata.ForEach(x => x.LibraryFileId = file.Id);

        _filamentDataContext.LibraryFileMetaData.RemoveRange(_filamentDataContext.LibraryFileMetaData.Where(x => x.LibraryFileId == file.Id));
        _filamentDataContext.LibraryFileMetaData.AddRange(metadata);
        _filamentDataContext.SaveChanges();
    }
}