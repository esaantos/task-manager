namespace TaskManager.Core.Entities;

public class TaskUpdatedHistory : BaseEntity
{
    public TaskUpdatedHistory(int taskId, int userId, string changeDescription)
    {
        TaskId = taskId;
        UserId = userId;
        ChangeDescription = changeDescription;

        UpdatedAt = DateTime.UtcNow;
    }

    public int TaskId { get; private set; }
    public int UserId { get; private set; }
    public string ChangeDescription { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User User { get; private set; }
    public Task Task { get; private set; }
}
