using filament.data.configurations;
using filament.data.models;

using Microsoft.EntityFrameworkCore;

namespace filament.data;

public class FilamentDataContext : DbContext
{
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryFile> LibraryFiles { get; set; }
    public DbSet<LibraryFileMetaData> LibraryFileMetaData { get; set; }
    public DbSet<QueueItem> TaskQueue { get; set; }

    public DbSet<TranscodeProfile> TranscodeProfiles { get; set; }

    public DbSet<TranscodeJob> TranscodeJobs { get; set; }
    public DbSet<SettingsItem> Settings { get; set; }

    public FilamentDataContext()
    {
    }

    public FilamentDataContext(DbContextOptions<FilamentDataContext> config) : base(config)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LibraryConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryFileConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryFileMetaDataConfiguration());
    }
}