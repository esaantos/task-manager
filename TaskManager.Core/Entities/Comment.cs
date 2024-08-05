namespace TaskManager.Core.Entities;

public class Comment : BaseEntity
{
    public Comment(string content, int taskId, int userId)
    {
        Content = content;
        TaskId = taskId;
        UserId = userId;

        CreatedAt = DateTime.Now;
    }

    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }
    public int TaskId { get; private set; }
    public Task Task { get; private set; }
}
