using TaskManager.Application.InputModels;
using TaskManager.Application.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskViewModel> GetTaskByIdAsync(int id);
    Task<ICollection<TaskViewModel>> GetTaskByProjectIdAsync(int projectId);
    Task<TaskViewModel> CreateTaskAsync(CreateTaskInputModel taskInputModel);
    Task UpdateTaskAsync(UpdateTaskInputModel taskInputModel);
    Task DeleteTaskAsync(int id);
    Task StartAsync(int id);
    Task FinishAsync(int id);
    Task AddCommentAsync(CreateCommentInputModel comment);
    Task<PerformanceReportViewModel> GetPerformanceReportAsync();
}
