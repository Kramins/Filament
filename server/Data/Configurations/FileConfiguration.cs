using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace filament.data.configurations;

public class LibraryFileConfiguration : IEntityTypeConfiguration<LibraryFile>
{
    public void Configure(EntityTypeBuilder<LibraryFile> builder)
    {
        builder.HasKey(f => f.Id);
        builder.HasAlternateKey(f => new { f.LibraryId, f.Path });

        builder.Property(f => f.EntityCreated)
            .HasDefaultValueSql("now()");

        builder.HasOne(f => f.Library)
            .WithMany(l => l.Files)
            .HasForeignKey(f => f.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}