using TaskManager.Core.Enum;
using Task = TaskManager.Core.Entities.Task;

namespace TaskManager.Application.ViewModels;

public class TaskViewModel
{
    public TaskViewModel(int id, string title, string description, DateTime dueDate, Priority priority, Status status, DateTime createdAt, int projectId)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = status;
        CreatedAt = createdAt;
        ProjectId = projectId;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public Priority Priority { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int ProjectId { get; private set; }

    public static TaskViewModel FromEntity(Task task)
        => new(task.Id, task.Title, task.Description, task.DueDate, task.Priority, task.Status, task.CreatedAt, task.ProjectId);
}
