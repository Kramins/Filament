using Microsoft.EntityFrameworkCore;
using filament.data;
using filament.data.models;
using filament.services;
using filament.scheduler;
using filament.tasks;

namespace filament;

public static class WebApplicationExtensions
{

    public static void AddFilamentDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<SchedulerClientService>();
        builder.Services.AddTransient<LibraryService>();
        builder.Services.AddTransient<DiskScanService>();
        builder.Services.AddTransient<TaskService>();
        builder.Services.AddTransient<FileMetaDataService>();

        builder.Services.AddScoped<ScanLibraryTask>();
    }

    public static void SetupDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        using (var context = scope.ServiceProvider.GetService<FilamentDataContext>())
        {
            var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
            if (context == null)
            {

                return;
            }
            var currentDatabaseVersion = context.Database.GetAppliedMigrations().LastOrDefault();
            var latestDatabaseVersion = context.Database.GetMigrations().LastOrDefault();
            logger.LogInformation($"Current database version: {currentDatabaseVersion}");
            if (currentDatabaseVersion != latestDatabaseVersion)
            {
                logger.LogInformation($"Database is out of date, migrating to {latestDatabaseVersion}");
                context.Database.Migrate();
            }

            if (!context.Settings.Any())
            {
                logger.LogInformation("No settings found, creating default settings");
                context.Settings.Add(new SettingsItem { Name = "FilamentConnectionString", Value = "Host=localhost;Port=5432;Database=filament;Username=filament;Password=filament" });
                context.Settings.Add(new SettingsItem { Name = "ScanInterval", Value = "60" });


                context.TranscodeProfiles.Add(new TranscodeProfile()
                {
                    Name = "h264",
                    Description = "h264",
                    VideoCodec = "h264"
                });

                context.TranscodeProfiles.Add(new TranscodeProfile()
                {
                    Name = "h265",
                    Description = "h265",
                    VideoCodec = "h265"
                });

                context.SaveChanges();
            }

            var settings = context.Settings.ToList();
            //Load settings into Microsoft.Extensions.Configuration.IConfiguration

            var configuration = scope.ServiceProvider.GetService<IConfiguration>();
            foreach (var setting in settings)
            {
                configuration[setting.Name] = setting.Value;
            }

        }
    }
}