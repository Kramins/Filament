using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server.Data.Configurations;

public class TranscodeJobConfiguration : IEntityTypeConfiguration<TranscodeJob>
{
    public void Configure(EntityTypeBuilder<TranscodeJob> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.EntityCreated)
            .HasDefaultValueSql("now()");

        builder.HasOne(t => t.LibraryFile)
            .WithMany(l => l.TranscodeJobs)
            .HasForeignKey(t => t.LibraryFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}