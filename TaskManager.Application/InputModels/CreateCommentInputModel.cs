namespace TaskManager.Application.InputModels;

public class CreateCommentInputModel
{
    public string Content { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
}
