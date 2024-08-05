using Task = TaskManager.Core.Entities.Task;

namespace TaskManager.Application.ViewModels;

public class PerformanceReportViewModel
{
    public PerformanceReportViewModel(List<Task> tasksList)
    {
        TasksList = tasksList;
        AverageTasksCompleted = TasksList.Count / 30.0;
    }

    public List<Task> TasksList { get; private set; }
    public double AverageTasksCompleted { get; private set; }
}
