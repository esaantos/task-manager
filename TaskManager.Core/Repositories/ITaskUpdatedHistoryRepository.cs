
using TaskManager.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Core.Repositories;

public interface ITaskUpdatedHistoryRepository
{
    Task UpdateTaskHistoryAsync(TaskUpdatedHistory taskHistory);
}
