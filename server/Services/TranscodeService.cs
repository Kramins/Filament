using filament.api.v1;
using filament.data;
using filament.data.models;
using filament.scheduler;
using filament.tasks;

namespace filament.services;

public class TranscodeService
{
    private ILogger<TranscodeService> _logger;
    private SchedulerClientService _schedulerClientService;
    private FilamentDataContext _dataContext;

    public TranscodeService(SchedulerClientService schedulerClientService, FilamentDataContext dataContext, ILogger<TranscodeService> logger)
    {
        _logger = logger;
        _schedulerClientService = schedulerClientService;
        _dataContext = dataContext;
    }

    public void SubmitTranscodeJob(int libraryId, int transcodeProfileId)
    {
        var transcodeJob = new TranscodeJob()
        {
            LibraryFileId = libraryId,
            TranscodeProfileId = transcodeProfileId,
            Status = "queued"
        };

        _dataContext.TranscodeJobs.Add(transcodeJob);
        _dataContext.SaveChanges();

        _schedulerClientService.PublishChannelTask<TranscodeTask>(t => t.Transcode(transcodeJob.Id), "transcode-tasks");
    }

    public TranscodeJob GetJob(int transcodeJobId)
    {
        return _dataContext.TranscodeJobs.SingleOrDefault(j => j.Id == transcodeJobId);
    }

    public void SubmitTranscodeJob(TranscodeJobRequestDto transcode)
    {
        throw new NotImplementedException();
    }
}