using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Repository.Common;

namespace Workflow.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IWorkflowDbContext context) : base(context)
        {
        }
    }
}