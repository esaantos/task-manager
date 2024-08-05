namespace TaskManager.Core.Entities;

public class Project : BaseEntity
{
    public Project(int userId, string name)
    {
        UserId = userId;
        Name = name;
        Removed = false;
        Task = [];
    }

    public string Name { get; private set; }
    public ICollection<Task> Task { get; private set; }
    public bool Removed { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    public void RemoveProject()
    {
        Removed = true;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
