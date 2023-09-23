using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace filament.data.configurations;

public class LibraryFileMetaDataConfiguration : IEntityTypeConfiguration<LibraryFileMetaData>
{
    public void Configure(EntityTypeBuilder<LibraryFileMetaData> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.EntityCreated)
            .HasDefaultValueSql("now()");

        builder.HasOne(f => f.LibraryFile)
            .WithMany(l => l.MetaData)
            .HasForeignKey(f => f.LibraryFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}