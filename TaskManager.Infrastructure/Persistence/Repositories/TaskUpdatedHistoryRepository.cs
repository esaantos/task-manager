using TaskManager.Core.Entities;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskUpdatedHistoryRepository : ITaskUpdatedHistoryRepository
{
    private readonly TaskManagerContext _context;

    public TaskUpdatedHistoryRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task UpdateTaskHistoryAsync(TaskUpdatedHistory taskHistory)
    {
        _context.TaskHistories.Add(taskHistory);
        await _context.SaveChangesAsync();
    }
}
