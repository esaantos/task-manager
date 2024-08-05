using System.Threading.Tasks;
using TaskManager.Application.Exceptions;
using TaskManager.Application.InputModels;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;
using TaskManager.Core.Enum;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<ProjectViewModel>> GetProjectsAsync()
    {
        var projects = await _repository.GetAllProjectsAsync();

        var projectViewModel = projects
            .Select(ProjectViewModel.FromEntity).ToList();

        return projectViewModel;
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(int id)
    {
        var project =  await GetAndValidateProjectAsync(id);

        return ProjectViewModel.FromEntity(project);
    }

    public async Task<ProjectViewModel> CreateProjectAsync(CreateProjectInputModel project)
    {
        var createProject = project.ToEntity();

        await _repository.AddProjectAsync(createProject);

        return ProjectViewModel.FromEntity(createProject);
    }

    public async Task UpdateProjectAsync(int id, UpdateProjectInputModel model)
    {
        var project = await GetAndValidateProjectAsync(id);

        project.Update(model.Name);

        await _repository.UpdateProjectAsync(project);
    }

    public async Task RemoveProjectAsync(int id)
    {
        var project = await GetAndValidateProjectAsync(id);

        if(project.Task.Any(t => t.Status != Status.Completed))
            throw new InvalidOperationException("Cannot remove project with pending tasks. Please complete or remove all tasks first.");

        project.RemoveProject();       
    }

    private async Task<Project> GetAndValidateProjectAsync(int id)
    {
        var project = await _repository.GetProjectByIdAsync(id);

        if(project is null)
            throw new NotFoundException($"Task with ID {id} not found.");

        return project;
    }
}