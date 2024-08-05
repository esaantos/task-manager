using TaskManager.Core.Entities;
using Task = TaskManager.Core.Entities.Task;

namespace TaskManager.Application.ViewModels;

public class ProjectViewModel
{
    public ProjectViewModel(int id, string name, ICollection<Task> tasks)
    {
        Id = id;
        Name = name;
        Tasks = tasks.Select(t => new TaskListViewModel(t.Id, t.Title, t.Description, t.Status)).ToList();
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<TaskListViewModel> Tasks { get; private set; }

    public static ProjectViewModel FromEntity(Project project)
        => new(project.Id, project.Name, project.Task);
}


