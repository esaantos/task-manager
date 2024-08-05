using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using Task = TaskManager.Core.Entities.Task;

namespace TaskManager.Infrastructure.Persistence;

public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options): base(options)
    {        
    }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskUpdatedHistory> TaskHistories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Project>(p =>
            {
                p.HasKey(p => p.Id);

                p.HasMany(p => p.Task)
                    .WithOne(t => t.Project)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(p => p.User)
                    .WithMany(u => u.Projects)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Restrict);             
            });


        modelBuilder
            .Entity<Task>(t =>
            {
                t.HasKey(t => t.Id);

                t.HasMany(t => t.Histories)
                    .WithOne(t => t.Task)
                    .HasForeignKey(t => t.TaskId);

                t.HasMany(t => t.Comments)
                    .WithOne(c => c.Task)
                    .HasForeignKey(c => c.TaskId);

                t.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

        modelBuilder
            .Entity<TaskUpdatedHistory>(tu =>
            {
                tu.HasKey(tu => tu.Id);

                tu.HasOne(h => h.Task)
                .WithMany(t => t.Histories)
                .HasForeignKey(h => h.TaskId);

            });

        modelBuilder
            .Entity<Comment>(c =>
            {
                c.HasKey(c => c.Id);

                c.HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId);
            });

        base.OnModelCreating(modelBuilder);
    }
}
