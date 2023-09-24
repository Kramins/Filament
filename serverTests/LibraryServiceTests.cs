using filament.data;
using filament.data.models;
using filament.scheduler;
using filament.services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Internal;

namespace serverTests;

public class LibraryServiceTests
{

    LibraryService _libraryService;

    //Mocks
    Mock<FilamentDataContext> _filamentDataContext;
    Mock<ILogger<LibraryService>> _logger;
    Mock<SchedulerClientService> _schedulerClientService;

    [SetUp]
    public void Setup()
    {
        _filamentDataContext = new Mock<FilamentDataContext>();
        _logger = new Mock<ILogger<LibraryService>>();
        _schedulerClientService = new Mock<SchedulerClientService>(new Mock<IOptions<SchedulerSettings>>().Object,_filamentDataContext.Object, new Mock<IServiceProvider>().Object, new Mock<ILogger < SchedulerClientService >>().Object);

       _libraryService = new LibraryService(_filamentDataContext.Object, _schedulerClientService.Object, _logger.Object);
    }
    [TearDown] public void Teardown() { }

    [Test]
    public void GetFileLibraryRealativePath_Test()
    {
        var library = new Library()
        {
            Id = 1,
            Name = "Test Library",
            Type = LibraryType.Videos,
            Location = "/data/videos/"
        };

        var filePath = "/data/videos/video.mp4";
        var result = _libraryService.GetFileLibraryRealativePath(library, filePath);
        Assert.AreEqual("/", result);
    }

    [Test]
    public void GetFileLibraryRealativePath_DeepPath_Test()
    {
        var library = new Library()
        {
            Id = 1,
            Name = "Test Library",
            Type = LibraryType.Videos,
            Location = "/data/videos/"
        };

        var filePath = "/data/videos/TV Shows/Show 1/Season 1/Episode 1.mp4";
        var result = _libraryService.GetFileLibraryRealativePath(library, filePath);
        Assert.AreEqual("TV Shows/Show 1/Season 1/", result);
    }
}
