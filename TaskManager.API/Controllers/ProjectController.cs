using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var projects = await _projectService.GetProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProjectInputModel model)
    {
        var project = await _projectService.CreateProjectAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = project.Id}, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectInputModel model)
    {
        await _projectService.UpdateProjectAsync(id, model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _projectService.RemoveProjectAsync(id);

        return NoContent();
    }
}
