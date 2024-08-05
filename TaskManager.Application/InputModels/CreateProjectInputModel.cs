using TaskManager.Core.Entities;

namespace TaskManager.Application.InputModels;

public class CreateProjectInputModel
{
    public string Name { get; set; }
    public int UserId { get; set; }

    public Project ToEntity()
        => new(UserId, Name);
}
