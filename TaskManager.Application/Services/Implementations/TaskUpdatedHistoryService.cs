using Microsoft.Extensions.Logging;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Core.Entities;
using TaskManager.Core.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services.Implementations;

public class TaskUpdatedHistoryService : ITaskUpdatedHistoryService
{
    private readonly ILogger<TaskUpdatedHistoryService> _logger;
    private readonly ITaskUpdatedHistoryRepository _repository;

    public TaskUpdatedHistoryService(ITaskUpdatedHistoryRepository repository, ILogger<TaskUpdatedHistoryService> logger)
    {
        _repository = repository;

        _logger = logger;
    }

    public async Task LogTaskUpdateAsync(int taskId, int idUser, Dictionary<string, (object oldValue, object newValue)> changes)
    {
        foreach (var change in changes)
        {
            var changeDescription = $"{change.Key} alterado de {change.Value.oldValue} para {change.Value.newValue}.";

            _logger.LogInformation(taskId, changeDescription);

            var history = new TaskUpdatedHistory(taskId, idUser, changeDescription);

            await _repository.UpdateTaskHistoryAsync(history);
        }
    }
}
