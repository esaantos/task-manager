using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Services.Implementations;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskUpdatedHistoryService, TaskUpdatedHistoryService>();
        return services;
    }
}
