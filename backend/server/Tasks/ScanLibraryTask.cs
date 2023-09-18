using filament.data;
using filament.data.models;
using filament.services;
using server.Data.Models;

namespace filament.tasks;
public class ScanLibraryTask
{

    private readonly ILogger<ScanLibraryTask> _logger;
    private readonly LibraryService _libraryService;
    private readonly DiskScanService _diskScanService;

    public ScanLibraryTask(LibraryService libraryService, DiskScanService diskScanService, ILogger<ScanLibraryTask> logger)
    {

        _libraryService = libraryService;
        _diskScanService = diskScanService;
        _logger = logger;
    }

    public void Scan(int libraryId)
    {
        _logger.LogInformation($"Scanning library {libraryId}");

        var library = _libraryService.GetLibrary(libraryId) ?? throw new Exception($"Library {libraryId} not found");

        switch (library.Type)
        {
            case LibraryType.Movies:
                ScanMovies(library);
                break;
            // case LibraryType.TV:
            //     ScanTV(library);
            //     break;
            default:
                throw new Exception($"Unknown library type {library.Type}");
        }
    }

    private void ScanMovies(Library library)
    {
        var libraryFileExtensions = new string[] { ".mkv", ".mp4", ".avi", ".mov", ".wmv", ".m4v" };
        var physicalFiles = _diskScanService.GetFilesOf(library.Location, libraryFileExtensions);

        var libraryFiles = _libraryService.GetFiles(library.Id).ToList();

        var newFiles = physicalFiles.Where(x => !libraryFiles.Any(y => y.Path == x.Path)).ToList();
        var deletedFiles = libraryFiles.Where(x => !physicalFiles.Any(y => y.Path == x.Path)).ToList();
        var existingFiles = from f in physicalFiles
                            join lf in libraryFiles on f.Path equals lf.Path
                            select new { PhysicalFile = f, LibraryFile = lf };

        foreach (var file in newFiles)
        {
            _logger.LogInformation($"Adding file {file.Path}");
            _libraryService.AddFile(library, new LibraryFile()
            {
                Path = file.Path,
                Name = file.Name,
                Extension = file.Extension,
                Size = file.Size,
                LastModified = file.LastModified
            });
        }




    }
}