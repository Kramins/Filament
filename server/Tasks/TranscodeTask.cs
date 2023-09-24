using filament.services;

namespace filament.tasks;

public class TranscodeTask
{
    private readonly TranscodeService _transcodeService;
    private readonly ILogger<TranscodeTask> _logger;

    public TranscodeTask(TranscodeService transcodeService, ILogger<TranscodeTask> logger)
    {
        _transcodeService = transcodeService;
        _logger = logger;
    }

    public void Transcode(int transcodeJobId)
    {
        _logger.LogInformation($"Transcoding job {transcodeJobId}");
        var job = _transcodeService.GetJob(transcodeJobId);
    }
}