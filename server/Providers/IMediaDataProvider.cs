
using filament.data.models;

namespace filament.providers.metadata;

public interface IFileMetaDataProvider
{
    Task<List<LibraryFileMetaData>> ScanAsync(string filePath);
}