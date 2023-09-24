using Microsoft.Extensions.Options;

namespace filament.scheduler;

public class SchedulerHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulerHostedService> _logger;
    private readonly IOptions<SchedulerSettings> _settings;

    private Dictionary<string, SchedulerChannelWorker> _channels = new Dictionary<string, SchedulerChannelWorker>();

    public SchedulerHostedService(IServiceProvider serviceProvider, IOptions<SchedulerSettings> schedulerSettings, ILogger<SchedulerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = schedulerSettings;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var backgroundTaskChannelName = "background-tasks";
        var backgroundChannel = new SchedulerChannelWorker(backgroundTaskChannelName, _serviceProvider, _logger);
        _channels.Add(backgroundTaskChannelName, backgroundChannel);

        var transcodeTaskChannelName = "transcode-tasks";
        var transcodeChannel = new SchedulerChannelWorker(transcodeTaskChannelName, _serviceProvider, _logger);
        _channels.Add(transcodeTaskChannelName, transcodeChannel);

        _channels.Values.ToList().ForEach(c => c.Start());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channels.Values.ToList().ForEach(c => c.Stop());
        return Task.CompletedTask;
    }
}