using Microsoft.EntityFrameworkCore;
using filament.data;
using filament.data.models;
using filament.services;

namespace filament;

public static class WebApplicationExtensions
{

    public static void AddFilamentDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<LibraryService>();
    }

    public static void SetupDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        using (var context = scope.ServiceProvider.GetService<FilamentDataContext>())
        {
            if (context == null)
            {
                return;
            }

            context.Database.Migrate();
        }
    }
}