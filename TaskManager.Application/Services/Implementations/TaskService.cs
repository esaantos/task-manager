using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;
using System.Reflection;
using TaskManager.Application.Exceptions;
using Microsoft.VisualBasic;

namespace TaskManager.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskUpdatedHistoryService _taskUpdatedHistoryService;

    public TaskService(ITaskRepository taskRepository, ITaskUpdatedHistoryService taskUpdatedHistoryService, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _taskUpdatedHistoryService = taskUpdatedHistoryService;
        _projectRepository = projectRepository;
    }

    public async Task<TaskViewModel> GetTaskByIdAsync(int id)
    {
        var task = await GetAndValidateTaskAsync(id);

        return TaskViewModel.FromEntity(task);
    }

    public async Task<ICollection<TaskViewModel>> GetTaskByProjectIdAsync(int projectId)
    {
        var tasksList = await _taskRepository.GetTasksAsync(projectId);

        if (tasksList is null)
        {
            throw new NotFoundException($"Project with ID {projectId} not found.");
        }

        return tasksList.Select(t => new TaskViewModel(t.Id, t.Title, t.Description, t.DueDate, t.Priority,
                                                        t.Status, t.CreatedAt, t.ProjectId)).ToList();
    }

    public async Task<TaskViewModel> CreateTaskAsync(CreateTaskInputModel model)
    {
        var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);

        if (project is null)
        {
            throw new NotFoundException($"Project with ID {model.ProjectId} not found.");
        }

        else if (project.Task.Count() >= 20)
        {
            throw new InvalidOperationException("Cannot add more than 20 tasks to a project");
        }

        var task = model.ToEntity(project.UserId);

        await _taskRepository.AddTaskAsync(task);

        return TaskViewModel.FromEntity(task);
    }

    public async Task UpdateTaskAsync(UpdateTaskInputModel model)
    {
        var task = await GetAndValidateTaskAsync(model.Id);

        var oldTask = new { Title = task.Title, Description = task.Description, DueDate = task.DueDate};
        task.Update(model.Title, model.Description, model.DueDate);

        await _taskRepository.UpdateTaskAsync(task);
        var changes = CompareValues(oldTask, task);

        if(changes.Any())
        {
            await _taskUpdatedHistoryService.LogTaskUpdateAsync(task.Id, task.UserId, changes);
        }

    }

    public async Task StartAsync(int id)
    {
        var task = await GetAndValidateTaskAsync(id);

        var oldStatus = task.Status;

        task.Start();
        await _taskRepository.UpdateTaskAsync(task);

        var change = CompareValues(new { Status = oldStatus }, task);
        if (change.Any())
        {
            await _taskUpdatedHistoryService.LogTaskUpdateAsync(task.Id, task.UserId, change);
        }
    }

    public async Task FinishAsync(int id)
    {
        var task = await GetAndValidateTaskAsync(id);

        var oldStatus = task.Status;
        task.Finish();

        await _taskRepository.UpdateTaskAsync(task);

        var change = CompareValues(new { Status = oldStatus }, task);
        if (change.Any())
        {
            await _taskUpdatedHistoryService.LogTaskUpdateAsync(task.Id, task.UserId, change);
        }
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await GetAndValidateTaskAsync(id);

        await _taskRepository.DeleteTaskAsync(id);
    }

    public async Task AddCommentAsync(CreateCommentInputModel model)
    {
        var task = await GetAndValidateTaskAsync(model.TaskId);

        var comment = new Comment(model.Content, model.TaskId, model.UserId);

        task.AddComment(comment);

        await _taskRepository.UpdateTaskAsync(task);

        var change = CompareValues(new { Comment = comment }, task);
        if (change.Any())
        {
            await _taskUpdatedHistoryService.LogTaskUpdateAsync(task.Id, task.Project.UserId, change);
        }
    }

    public async Task<PerformanceReportViewModel> GetPerformanceReportAsync()
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        var tasks = await _taskRepository.GetTasksCompletedAsync(thirtyDaysAgo);

        return new PerformanceReportViewModel(tasks);
    }

    private Dictionary<string, (object oldValue, object newValue)> CompareValues<T>(T model, Core.Entities.Task task) where T : class
    {
        var changes = new Dictionary<string, (object oldValue, object newValue)>();
        var modelProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var taskProperties = typeof(Core.Entities.Task).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var modelProperty in modelProperties)
        {
            var taskProperty = taskProperties.FirstOrDefault(p => p.Name == modelProperty.Name);
            if (taskProperty != null)
            {
                var modelValue = modelProperty.GetValue(model);
                var taskValue = taskProperty.GetValue(task);

                if (!Equals(modelValue, taskValue))
                {
                    changes.Add(modelProperty.Name, (modelValue, taskValue));
                }
            }
        }

        return changes;
    }

    private async Task<Core.Entities.Task> GetAndValidateTaskAsync(int taskId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);

        if (task == null)
        {
            throw new NotFoundException($"Task with ID {taskId} not found.");
        }

        return task;
    }
}
