using TaskManager.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Core.Repositories;

public interface IProjectRepository
{
    Task<ICollection<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProject(int id);
}
