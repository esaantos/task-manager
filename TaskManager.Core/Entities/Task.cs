using TaskManager.Core.Enum;

namespace TaskManager.Core.Entities;

public class Task : BaseEntity
{
    public Task(string title, string description, DateTime dueDate, Priority priority, int projectId, int userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        ProjectId = projectId;
        UserId = userId;

        Status = Status.Pending;
        CreatedAt = DateTime.Now;
        Histories = [];
        Comments = [];
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public Priority Priority { get; private set; }
    public Status Status { get; private set; }
    public ICollection<TaskUpdatedHistory> Histories { get; private set; }
    public ICollection<Comment> Comments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; private set; }

    public int ProjectId { get; private set; }
    public Project Project { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }

    public void Update(string title, string description, DateTime dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    public void Start()
    { 
        if(Status == Status.Pending)
        {
            Status = Status.InProgress;
        }
    }
    public void Finish()
    {
        if(Status == Status.InProgress)
        {
            Status = Status.Completed;
            CompletedAt = DateTime.Now;
        }
    }
    
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
}
