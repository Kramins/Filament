using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server.Data.Configurations;

public class SettingsConfiguration : IEntityTypeConfiguration<SettingsItem>
{
    public void Configure(EntityTypeBuilder<SettingsItem> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.EntityCreated)
            .HasDefaultValueSql("now()");
    }
}