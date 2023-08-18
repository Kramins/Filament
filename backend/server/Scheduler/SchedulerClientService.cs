using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Newtonsoft.Json;
using filament.tasks;

namespace filament.scheduler;

public class SchedulerClientService
{

    IOptions<SchedulerSettings> _settings;
    private ConnectionMultiplexer _redis;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulerClientService> _logger;
    private RedisChannel _backgroundTasksChannel;

    public SchedulerClientService(IOptions<SchedulerSettings> settings, IServiceProvider serviceProvider, ILogger<SchedulerClientService> logger)
    {
        _settings = settings;
        _redis = ConnectionMultiplexer.Connect(_settings.Value.ConnectionString);
        _serviceProvider = serviceProvider;
        _logger = logger;

        _backgroundTasksChannel = RedisChannel.Literal("BackgroundTasks");


    }

    public void PublishBackgroundTask<T>(Expression<Action<T>> methodCall)
    {
        if (!CheckTypeIsInScope(typeof(T)))
        {
            _logger.LogError("Type {Type} is not in scope", typeof(T));
            throw new Exception($"Type {typeof(T)} is not in scope");
            return;
        }

        var job = ScheduleTask.FromExpression(methodCall, typeof(T));
        var serializedJob = JsonConvert.SerializeObject(job);

        _redis.GetSubscriber().Publish(_backgroundTasksChannel, serializedJob);
    }

    private bool CheckTypeIsInScope(Type type)
    {
        return _serviceProvider.GetService(type) != null;
    }


}