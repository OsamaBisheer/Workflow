using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Repository.Common;

namespace Workflow.Repository
{
    public class ApplicationRoleDescriptionRepository : GenericRepository<ApplicationRoleDescription>, IApplicationRoleDescriptionRepository
    {
        public ApplicationRoleDescriptionRepository(IWorkflowDbContext context) : base(context)
        {
        }
    }
}