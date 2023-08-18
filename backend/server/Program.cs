using System.Reflection;
using filament;
using filament.data;
using filament.scheduler;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();


});

builder.Services.AddDbContext<FilamentDataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("FilamentConnectionString"));
});


builder.Services.Configure<SchedulerSettings>(c =>
{
    c.Interval = 1000;
    c.ConnectionString = "redis:6379";
});
builder.Services.AddHostedService<SchedulerHostedService>();

builder.AddFilamentDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SetupDatabase();



app.Run();
