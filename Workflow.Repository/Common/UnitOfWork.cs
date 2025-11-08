using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;

namespace Workflow.Repository.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IWorkflowDbContext Context { get; }
        public IActionTypeRepository ActionTypes { get; private set; }
        public IApplicationRoleDescriptionRepository ApplicationRoleDescriptions { get; private set; }
        public IApplicationUserRoleRepository ApplicationUserRoles { get; private set; }
        public IProcessRepository Processes { get; private set; }
        public IStepRepository Steps { get; private set; }
        public IUserRepository Users { get; private set; }
        public IWorkflowRepository Workflows { get; private set; }

        public UnitOfWork(IWorkflowDbContext _context)
        {
            Context = _context;

            ActionTypes = new ActionTypeRepository(_context);
            ApplicationRoleDescriptions = new ApplicationRoleDescriptionRepository(_context);
            ApplicationUserRoles = new ApplicationUserRoleRepository(_context);
            Processes = new ProcessRepository(_context);
            Steps = new StepRepository(_context);
            Users = new UserRepository(_context);
            Workflows = new WorkflowRepository(_context);
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public async Task<int> Commit()
        {
            // Save changes with the default options
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}