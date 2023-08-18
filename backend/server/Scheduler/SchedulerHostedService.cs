using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace filament.scheduler;
public class SchedulerHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulerHostedService> _logger;
    private readonly IOptions<SchedulerSettings> _settings;
    private Timer? _timer = null;
    private int executionCount = 0;
    private ConnectionMultiplexer _redis;
    private readonly RedisChannel _backgroundTasksChannel;

    public SchedulerHostedService(IServiceProvider serviceProvider, IOptions<SchedulerSettings> schedulerSettings, ILogger<SchedulerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = schedulerSettings;
        _redis = ConnectionMultiplexer.Connect(_settings.Value.ConnectionString);

        _backgroundTasksChannel = RedisChannel.Literal("BackgroundTasks");


    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_settings.Value.Interval));

        var backgroundTaskQueue = _redis.GetSubscriber().Subscribe(_backgroundTasksChannel, CommandFlags.FireAndForget);

        backgroundTaskQueue.OnMessage(message => ProcessBackgroundTask(message.Message));

        return Task.CompletedTask;
    }

    private void ProcessBackgroundTask(RedisValue message)
    {
        var task = JsonConvert.DeserializeObject<ScheduleTask>(message.ToString());

        _logger.LogInformation("Processing background task: {Task}", task);

        using (var scope = _serviceProvider.CreateScope())
        {
            var taskType = Type.GetType(task.Type);

            var taskInstance = scope.ServiceProvider.GetRequiredService(taskType);
            // var taskInstance = Activator.CreateInstance(taskType);

            var method = taskType.GetMethod(task.Method);

            var types = task.ArgumentTypes.Select(x => Type.GetType(x)).ToArray();
            var castedParameters = task.Arguments.Zip(types, (arg, type) => JsonConvert.DeserializeObject(arg.ToString(), type)).ToArray();

            method.Invoke(taskInstance, castedParameters);
        }
        _logger.LogInformation("Finished processing background task: {Task}", task);
    }

    private void DoWork(object? state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}