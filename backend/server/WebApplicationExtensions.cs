using Microsoft.EntityFrameworkCore;
using filament.data;


public static class WebApplicationExtensions
{

    public static void SetupDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        using (var context = scope.ServiceProvider.GetService<FilamentDataContext>())
        {
            // context.Database.EnsureCreated();
            context.Database.Migrate();
        }
    }
}