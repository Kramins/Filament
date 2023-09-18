
using filament.data;
using filament.scheduler;
using filament.services;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;

namespace filament.api.v1;

[Route("api/v1/system/tasks")]

public class TasksController
{
    private readonly ILogger<TasksController> _logger;
    private readonly TaskService _taskService;

    public TasksController(ILogger<TasksController> logger, TaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }

    [HttpPost("scan-library")]
    public ApiResponse<ScanLibraryResponseDto> ScanLibrary([FromBody] ScanLibraryRequestDto request)
    {
        _taskService.ScanLibrary(request.Id);
        return new ApiResponse<ScanLibraryResponseDto>(new ScanLibraryResponseDto());
    }
}