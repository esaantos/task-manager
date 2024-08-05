namespace TaskManager.Core.Entities;

public class User : BaseEntity
{
    public User(string name, string role)
    {
        Name = name;
        Role = role;
    }

    public string Name { get; private set; }
    public string Role { get; private set; }

    public ICollection<Project> Projects { get; private set; }
    public ICollection<Task> Tasks { get; private set; }
}
