using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services.Interfaces;

public interface ITaskUpdatedHistoryService
{
    Task LogTaskUpdateAsync(int taskId, int idUser, Dictionary<string, (object oldValue, object newValue)> changes);
}
