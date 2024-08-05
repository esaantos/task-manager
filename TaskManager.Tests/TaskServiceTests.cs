using FluentAssertions;
using Moq;
using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Implementations;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Core.Entities;
using TaskManager.Core.Enum;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ITaskUpdatedHistoryService> _taskUpdatedHistoryServiceMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _taskUpdatedHistoryServiceMock = new Mock<ITaskUpdatedHistoryService>();
        _taskService = new TaskService(_taskRepositoryMock.Object, _taskUpdatedHistoryServiceMock.Object, _projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ExistingTask_ReturnsTask()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Test Task", "Description test", DateTime.Now, Priority.Medium, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task GetTaskByProjectIdAsync_ExistingTasks_ReturnsTasks()
    {
        // Arrange
        var tasks = new List<TaskManager.Core.Entities.Task>
        {
            new TaskManager.Core.Entities.Task("Test Task 1", "Description test", DateTime.Now, Priority.Medium, 1, 1),
            new TaskManager.Core.Entities.Task("Test Task 2", "Description test", DateTime.Now, Priority.Medium, 2, 1)
        };
        _taskRepositoryMock.Setup(repo => repo.GetTasksAsync(1)).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetTaskByProjectIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateTaskAsync_ValidInput_ReturnsCreatedTask()
    {
        // Arrange
        var project = new Project(1, "Project test");
        _projectRepositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync(project);
        var createTaskInput = new CreateTaskInputModel
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.Now,
            Priority = Priority.Medium,
            ProjectId = 1
        };

        // Act
        var result = await _taskService.CreateTaskAsync(createTaskInput);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Task");
        _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(It.IsAny<TaskManager.Core.Entities.Task>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ValidInput_UpdatesTask()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Old Title", "Old Description", DateTime.Now.AddDays(-1), Priority.High, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);
        var updateTaskInput = new UpdateTaskInputModel
        {
            Id = 1,
            Title = "New Title",
            Description = "New Description",
            DueDate = DateTime.Now
        };

        // Act
        await _taskService.UpdateTaskAsync(updateTaskInput);

        // Assert
        _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskManager.Core.Entities.Task>()), Times.Once);
        _taskUpdatedHistoryServiceMock.Verify(service => service.LogTaskUpdateAsync(task.Id, task.UserId, It.IsAny<Dictionary<string, (object oldValue, object newValue)>>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_ValidTask_StartsTask()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Test Task", "Description test", DateTime.Now, Priority.Medium, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        await _taskService.StartAsync(1);

        // Assert
        Assert.Equal(Status.InProgress, task.Status);
        _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskManager.Core.Entities.Task>()), Times.Once);
        _taskUpdatedHistoryServiceMock.Verify(service => service.LogTaskUpdateAsync(task.Id, task.UserId, It.IsAny<Dictionary<string, (object oldValue, object newValue)>>()), Times.Once);
    }

    [Fact]
    public async Task FinishAsync_ValidTask_FinishesTask()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Test Task 1", "Description test", DateTime.Now, Priority.Medium, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        await _taskService.StartAsync(1);
        await _taskService.FinishAsync(1);

        // Assert
        Assert.Equal(Status.Completed, task.Status);
        _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskManager.Core.Entities.Task>()), Times.Once);
        _taskUpdatedHistoryServiceMock.Verify(service => service.LogTaskUpdateAsync(task.Id, task.UserId, It.IsAny<Dictionary<string, (object oldValue, object newValue)>>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_ExistingTask_DeletesTask()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Test Task 1", "Description test", DateTime.Now, Priority.Medium, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        await _taskService.DeleteTaskAsync(1);

        // Assert
        _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddCommentAsync_ValidComment_AddsComment()
    {
        // Arrange
        var task = new TaskManager.Core.Entities.Task("Test Task 1", "Description test", DateTime.Now, Priority.Medium, 1, 1);
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);
        var createCommentInput = new CreateCommentInputModel
        {
            TaskId = 1,
            Content = "New Comment",
            UserId = 1
        };

        // Act
        await _taskService.AddCommentAsync(createCommentInput);

        // Assert
        _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskManager.Core.Entities.Task>()), Times.Once);
        _taskUpdatedHistoryServiceMock.Verify(service => service.LogTaskUpdateAsync(task.Id, task.Project.UserId, It.IsAny<Dictionary<string, (object oldValue, object newValue)>>()), Times.Once);
    }
}
