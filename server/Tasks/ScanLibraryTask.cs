using filament.data.models;
using filament.services;

namespace filament.tasks;

public class ScanLibraryTask
{
    private readonly ILogger<ScanLibraryTask> _logger;
    private readonly LibraryService _libraryService;
    private readonly FileMetaDataService _fileMetaDataService;

    public ScanLibraryTask(LibraryService libraryService, FileMetaDataService fileMetaDataService, ILogger<ScanLibraryTask> logger)
    {
        _libraryService = libraryService;
        _fileMetaDataService = fileMetaDataService;
        _logger = logger;
    }

    public void Scan(int libraryId)
    {
        _logger.LogInformation($"Scanning library {libraryId}");

        var library = _libraryService.GetLibrary(libraryId) ?? throw new Exception($"Library {libraryId} not found");

        switch (library.Type)
        {
            case LibraryType.Videos:
                ScanVideos(library);
                break;

            // case LibraryType.TV:
            //     ScanTV(library);
            //     break;
            default:
                throw new Exception($"Unknown library type {library.Type}");
        }
    }

    private void ScanVideos(Library library)
    {
        var libraryFileExtensions = new string[] { ".mkv", ".mp4", ".avi", ".mov", ".wmv", ".m4v" };

        var libraryFiles = _libraryService.GetFiles(library.Id).ToList();

        WalkDirectory(library.Location, libraryFiles, library, libraryFileExtensions);
    }

    private void WalkDirectory(string path, List<LibraryFile> libraryFiles, Library library, string[] libraryFileExtensions)
    {
        _logger.LogInformation($"Scanning directory {path}");
        var files = Directory.GetFiles(path);

        var libraryDirectoryInfo = libraryFiles.FirstOrDefault(x => x.Path == path);
        var directoryInfo = new DirectoryInfo(path);
        if (libraryDirectoryInfo == null)
        {
            libraryDirectoryInfo = new LibraryFile()
            {
                Path = _libraryService.GetFileLibraryRealativePath(library, path) + "/", // Add trailing slash
                Name = directoryInfo.Name,
                IsDirectory = true,
                Size = 0,
                LastModified = directoryInfo.LastWriteTimeUtc,
            };
            _libraryService.AddFile(library, libraryDirectoryInfo);
        }
        else
        {
            libraryDirectoryInfo.Size = 0;
            libraryDirectoryInfo.LastModified = directoryInfo.LastWriteTimeUtc;
            _libraryService.UpdateFile(library, libraryDirectoryInfo);
        }
        // Delete files that no longer exist
        foreach (var libraryFile in libraryFiles.Where(x => x.Path.StartsWith(libraryDirectoryInfo.Path)))
        {
            if (!files.Contains(_libraryService.GetFullFilePath(library, libraryFile.Path)))
            {
                _libraryService.DeleteFile(library, libraryFile);
            }
        }
        // Add of update files
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file);
            if (!libraryFileExtensions.Contains(extension))
            {
                _logger.LogTrace($"Skipping file {file} as it is not an allowed file type");
                continue;
            }
            var fileInfo = new FileInfo(file);
            var libraryFilePath = _libraryService.GetFileLibraryRealativePath(library, file);
            var libraryFile = libraryFiles.FirstOrDefault(x => x.Path == libraryFilePath);

            if (libraryFile == null)
            {
                libraryFile = new LibraryFile()
                {
                    Path = libraryFilePath,
                    IsDirectory = false,
                    Name = fileInfo.Name,
                    Extension = fileInfo.Extension,
                    Size = fileInfo.Length,
                    LastModified = fileInfo.LastWriteTimeUtc,
                };
                _libraryService.AddFile(library, libraryFile);
            }
            else
            {
                libraryFile.Size = fileInfo.Length;
                libraryFile.LastModified = fileInfo.LastWriteTimeUtc;
                _libraryService.UpdateFile(library, libraryFile);
            }

            libraryDirectoryInfo.Size += fileInfo.Length;
            _libraryService.UpdateFile(library, libraryDirectoryInfo);
            _fileMetaDataService.ScanFile(library, libraryFile);
        }

        var directories = Directory.GetDirectories(path);
        foreach (var directory in directories)
        {
            WalkDirectory(directory, libraryFiles, library, libraryFileExtensions);
        }
    }
}