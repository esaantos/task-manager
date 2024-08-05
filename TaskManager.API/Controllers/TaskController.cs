using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        return Ok(task);
    }

    [HttpGet("{projectId}/project")]
    public async Task<IActionResult> GetByIdProject(int projectId)
    {
        var task = await _taskService.GetTaskByProjectIdAsync(projectId);

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskInputModel model)
    {
        var task = await _taskService.CreateTaskAsync(model);

        return CreatedAtAction(nameof(GetById), new { Id = task.Id }, task);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentInputModel model)
    {
        await _taskService.AddCommentAsync(model);

        return CreatedAtAction(nameof(GetById), new { id, model });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromBody] UpdateTaskInputModel model)
    {
        await _taskService.UpdateTaskAsync(model);

        return NoContent();
    }

    [HttpPut("{id}/start")]
    public async Task<IActionResult> Start(int id)
    {
        await _taskService.StartAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/finish")]
    public async Task<IActionResult> Finish(int id)
    {
        await _taskService.FinishAsync(id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskService.DeleteTaskAsync(id);

        return NoContent();
    }
}
