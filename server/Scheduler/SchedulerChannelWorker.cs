using filament.data;
using filament.data.models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace filament.scheduler;
public class SchedulerChannelWorker
{

    Timer _timer = null;

    List<Task> _tasks = new List<Task>();
    int _maxConcurrency = 1;
    public SchedulerChannelWorker(string channelName, IServiceProvider serviceProvider, ILogger<SchedulerHostedService> logger)
    {
        this.channelName = channelName;
        _serviceProvider = serviceProvider;
        _Interval = TimeSpan.FromSeconds(1);
        _logger = logger;

    }
    public void Start()
    {
        _logger.LogInformation("Starting channel {Channel}", channelName);
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    public void Stop()
    {
        _logger.LogInformation("Stopping channel {Channel}", channelName);
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        _timer = null;
    }
    private void ProcessBackgroundTask(QueueItem message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dataContext = scope.ServiceProvider.GetService<FilamentDataContext>();

            var task = JsonConvert.DeserializeObject<ScheduleTask>(message.Payload.RootElement.GetRawText());

            _logger.LogInformation("Processing background task: {Task}", task);


            var taskType = Type.GetType(task.Type);

            var taskInstance = scope.ServiceProvider.GetRequiredService(taskType);
            // var taskInstance = Activator.CreateInstance(taskType);

            var method = taskType.GetMethod(task.Method);

            var types = task.ArgumentTypes.Select(x => Type.GetType(x)).ToArray();
            var castedParameters = task.Arguments.Zip(types, (arg, type) => JsonConvert.DeserializeObject(arg.ToString(), type)).ToArray();

            try
            {
                method.Invoke(taskInstance, castedParameters);
                message.Status = "complete";
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing background task");
                message.Status = "error";
                message.Error = e.Message;
            }
            finally
            {
                message.Completed = DateTime.UtcNow;
                dataContext.Update(message);
                dataContext.SaveChanges();
                _logger.LogInformation("Finished processing background task: {Task}", task);
            }
        }
    }

    private void DoWork(object? state)
    {
        _timer?.Change(Timeout.Infinite, 0);
        // Clean up any completed tasks
        _tasks.RemoveAll(t => t.IsCompleted);
        if (_tasks.Count >= _maxConcurrency)
        {
            _logger.LogTrace("Max concurrency reached for channel {Channel}", channelName);
            _timer?.Change(_Interval, _Interval);
            return;
        }

        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetService<FilamentDataContext>();

                using (var transaction = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = dataContext.TaskQueue
                            .FromSqlRaw($"SELECT * FROM \"TaskQueue\" WHERE \"Status\" = 'queued' AND \"Channel\" = '{channelName}' FOR UPDATE SKIP LOCKED LIMIT 1")
                            .ToList()
                            .FirstOrDefault();

                        if (task != null)
                        {
                            task.Status = "processing";
                            task.Started = DateTime.UtcNow;
                            dataContext.SaveChanges();
                            transaction.Commit();
                            var t = Task.Run(() => ProcessBackgroundTask(task));
                        }
                        else
                        {
                            _logger.LogTrace("No background tasks to process");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error processing background task");
                        transaction.Rollback();
                    }

                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing background task");
        }

        _timer?.Change(_Interval, _Interval);
    }

    public string channelName { get; }

    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _Interval;
    private readonly ILogger<SchedulerHostedService> _logger;
}