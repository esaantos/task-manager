using Moq;
using System.Reflection;
using TaskManager.Application.Exceptions;
using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Implementations;
using TaskManager.Core.Entities;
using TaskManager.Core.Enum;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _repositoryMock = new Mock<IProjectRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _service = new ProjectService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetProjectsAsync_ReturnsAllProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project(1, "Project 1"),
                new Project(1, "Project 2")
            };
            _repositoryMock.Setup(repo => repo.GetAllProjectsAsync()).ReturnsAsync(projects);

            // Act
            var result = await _service.GetProjectsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Project 1", result.First().Name);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ProjectExists_ReturnsProject()
        {
            // Arrange
            var project = new Project(1, "Project 1");
            _repositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync(project);

            // Act
            var result = await _service.GetProjectByIdAsync(1);

            // Assert
            Assert.Equal("Project 1", result.Name);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ProjectDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync((Project)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetProjectByIdAsync(1));
        }

        [Fact]
        public async Task CreateProjectAsync_ValidProject_CreatesProject()
        {
            // Arrange
            var createProjectInput = new CreateProjectInputModel { Name = "New Project" };
            var project = new Project(1, "New Project");
            _repositoryMock.Setup(repo => repo.AddProjectAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateProjectAsync(createProjectInput);

            // Assert
            Assert.Equal("New Project", result.Name);
            _repositoryMock.Verify(repo => repo.AddProjectAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProjectAsync_ProjectExists_UpdatesProject()
        {
            // Arrange
            var project = new Project(1, "Old Project");
            var updateProjectInput = new UpdateProjectInputModel { Name = "Updated Project" };
            _repositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync(project);
            _repositoryMock.Setup(repo => repo.UpdateProjectAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateProjectAsync(1, updateProjectInput);

            // Assert
            Assert.Equal("Updated Project", project.Name);
            _repositoryMock.Verify(repo => repo.UpdateProjectAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task RemoveProjectAsync_ProjectWithPendingTasks_ThrowsInvalidOperationException()
        {
            // Arrange
            var project = new Project(1, "Project with tasks");
            var task = new Core.Entities.Task("test", "some test", DateTime.UtcNow, Priority.Medium, 1, 1);

            _repositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync(project);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RemoveProjectAsync(1));
        }

        [Fact]
        public async Task RemoveProjectAsync_ProjectWithoutPendingTasks_RemovesProject()
        {
            // Arrange
            var project = new Project(1, "Empty Project");
            _repositoryMock.Setup(repo => repo.GetProjectByIdAsync(1)).ReturnsAsync(project);

            // Act
            await _service.RemoveProjectAsync(1);

            // Assert
            Assert.True(project.Removed);
        }
    }

}
