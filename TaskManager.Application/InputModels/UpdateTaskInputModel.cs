﻿namespace TaskManager.Application.InputModels;

public class UpdateTaskInputModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
