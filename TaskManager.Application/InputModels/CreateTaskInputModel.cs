using TaskManager.Core.Enum;
using Task = TaskManager.Core.Entities.Task;

namespace TaskManager.Application.InputModels;

public class CreateTaskInputModel
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }

    public Task ToEntity(int userId)
        => new(Title, Description, DueDate, Priority, ProjectId, userId);
}
