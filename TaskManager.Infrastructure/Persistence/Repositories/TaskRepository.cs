using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Repositories;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerContext _context;

    public TaskRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Core.Entities.Task>> GetTasksAsync(int id)
    {
        return await _context.Tasks.Where(t => t.ProjectId == id).Include(p => p.Project).ToListAsync();
    }

    public async Task<Core.Entities.Task> GetTaskByIdAsync(int taskId)
    {
        return await _context.Tasks.SingleOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task AddTaskAsync(Core.Entities.Task task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTaskAsync(Core.Entities.Task task)
    {
        _context.Tasks.Update(task);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        _context.Remove(id);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Core.Entities.Task>> GetTasksCompletedAsync(DateTime from)
    {
        return await _context.Tasks
            .Where(t => t.Status == Core.Enum.Status.Completed && t.CompletedAt >= from)
            .ToListAsync();
    }
}
