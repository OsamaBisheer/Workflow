using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Repository.Common;

namespace Workflow.Repository
{
    public class ApplicationUserRoleRepository : GenericRepository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        public ApplicationUserRoleRepository(IWorkflowDbContext context) : base(context)
        {
        }
    }
}