using TaskManager.Core.Enum;

namespace TaskManager.Application.ViewModels;

public class TaskListViewModel
{
    public TaskListViewModel(int id, string title, string description, Status status)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; }
}
