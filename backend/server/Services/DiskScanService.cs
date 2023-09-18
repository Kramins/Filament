

namespace filament.services;

public class DiskScanService
{

    public DiskScanService()
    {

    }

    public IEnumerable<DiskScanFileInfo> GetFilesOf(string path, string[] extension)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Directory {path} not found");
        }

        var allFiles = Directory.EnumerateFiles(path, "*", new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true
        });

        var filteredFileList = allFiles.Where(file => extension.Contains(Path.GetExtension(file)))
                               .ToList();

        var fileInfoList = from f in filteredFileList
                           let fi = new FileInfo(f)
                           select new DiskScanFileInfo()
                           {
                               Name = fi.Name,
                               Path = fi.DirectoryName,
                               Size = fi.Length,
                               Extension = fi.Extension,
                               LastModified = fi.LastWriteTimeUtc
                           };
        return fileInfoList;
    }

    public bool Exists(string location)
    {
        return Directory.Exists(location);
    }
}
