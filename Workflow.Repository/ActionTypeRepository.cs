using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Repository.Common;

namespace Workflow.Repository
{
    public class ActionTypeRepository : GenericRepository<ActionType>, IActionTypeRepository
    {
        public ActionTypeRepository(IWorkflowDbContext context) : base(context)
        {
        }
    }
}