using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;

namespace Workflow.Domain.Interfaces.ICore
{
    public interface IWorkflowDbContext : IDisposable
    {
        DbSet<ApplicationRole> ApplicationRoles { get; set; }
        DbSet<ApplicationRoleDescription> ApplicationRoleDescriptions { get; set; }
        DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<ActionType> ActionTypes { get; set; }
        DbSet<Process> Processes { get; set; }
        DbSet<Step> Steps { get; set; }
        DbSet<Entities.Workflow> Workflows { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}