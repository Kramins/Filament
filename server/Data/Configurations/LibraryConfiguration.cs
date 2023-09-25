namespace server.Data.Configurations;

using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasAlternateKey(l => l.Name);

        builder.Property(l => l.EntityCreated)
            .HasDefaultValueSql("now()");
    }
}