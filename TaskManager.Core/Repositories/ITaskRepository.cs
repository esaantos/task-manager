using Task = System.Threading.Tasks.Task;

namespace TaskManager.Core.Repositories;

public interface ITaskRepository
{
    Task<ICollection<Entities.Task>> GetTasksAsync(int id);
    Task<Entities.Task> GetTaskByIdAsync(int taskId);
    Task AddTaskAsync(Entities.Task task);
    Task UpdateTaskAsync(Entities.Task task);
    Task DeleteTaskAsync(int id);
    Task<List<Entities.Task>> GetTasksCompletedAsync(DateTime from);
}
