using filament.data.configurations;
using filament.data.models;


using Microsoft.EntityFrameworkCore;

namespace filament.data;

public class FilamentDataContext : DbContext
{

    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryFile> LibraryFiles { get; set; }

    public FilamentDataContext(DbContextOptions<FilamentDataContext> config) : base(config)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LibraryConfiguration());
    }
}