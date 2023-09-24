using filament.data;
using filament.data.models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json;

namespace filament.scheduler;

public class SchedulerClientService
{
    private readonly FilamentDataContext _dataContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulerClientService> _logger;

    public SchedulerClientService(IOptions<SchedulerSettings> settings, FilamentDataContext dataContext, IServiceProvider serviceProvider, ILogger<SchedulerClientService> logger)
    {
        _dataContext = dataContext;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void EmitEvent(string eventName, object data)
    {
        var serializedData = JsonConvert.SerializeObject(data);
    }

    public Guid PublishChannelTask<T>(Expression<Action<T>> methodCall, string channel)
    {
        if (!CheckTypeIsInScope(typeof(T)))
        {
            _logger.LogError("Type {Type} is not in scope", typeof(T));
            throw new Exception($"Type {typeof(T)} is not in scope");
        }

        var job = ScheduleTask.FromExpression(methodCall, typeof(T));
        var serializedJob = JsonConvert.SerializeObject(job);

        var queueItem = new QueueItem()
        {
            Channel = channel,
            Payload = JsonDocument.Parse(serializedJob),
            Status = "queued"
        };

        _dataContext.TaskQueue.Add(queueItem);
        _dataContext.SaveChanges();
        return queueItem.Id;
    }

    private bool CheckTypeIsInScope(Type type)
    {
        return _serviceProvider.GetService(type) != null;
    }
}