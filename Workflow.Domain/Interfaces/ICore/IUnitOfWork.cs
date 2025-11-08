using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.IRepositories;

namespace Workflow.Domain.Interfaces.ICore
{
    public interface IUnitOfWork : IDisposable
    {
        IActionTypeRepository ActionTypes { get; }
        IApplicationRoleDescriptionRepository ApplicationRoleDescriptions { get; }
        IApplicationUserRoleRepository ApplicationUserRoles { get; }
        IProcessRepository Processes { get; }
        IStepRepository Steps { get; }
        IUserRepository Users { get; }
        IWorkflowRepository Workflows { get; }

        IWorkflowDbContext Context { get; }

        Task<int> Commit();
    }
}