
using filament.data.models;
using filament.scheduler;
using filament.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace filament.api.v1;

[ApiController]
[Route("api/v1/transcode")]
public class TranscodeController
{
    private readonly TranscodeService _transcodeService;

    public TranscodeController(TranscodeService transcodeService)
    {
        _transcodeService = transcodeService;
    }

    [HttpPost]
    public ApiResponse<TranscodeDto> Transcode(TranscodeDto transcode)
    {
        _transcodeService.SubmitTranscodeJob(transcode);
        return null;
    }

    // [HttpGet]
    // public ApiResponse<IEnumerable<TranscodeJobDto>> Get()
    // {
    //     return null;
    // }
}