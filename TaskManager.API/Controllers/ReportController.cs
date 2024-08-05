using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ITaskService _ITaskService;

    public ReportController(ITaskService reportService)
    {
        _ITaskService = reportService;
    }

    [HttpGet("performance")]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> Get()
    {
        var report = await _ITaskService.GetPerformanceReportAsync();
        return Ok(report);
    }

}
