using TaskManager.Application.InputModels;
using TaskManager.Application.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services.Interfaces;

public interface IProjectService
{
    Task<ICollection<ProjectViewModel>> GetProjectsAsync();
    Task<ProjectViewModel> GetProjectByIdAsync(int id);
    Task<ProjectViewModel> CreateProjectAsync(CreateProjectInputModel project);
    Task UpdateProjectAsync(int id, UpdateProjectInputModel project);
    Task RemoveProjectAsync(int id);
}
