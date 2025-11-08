using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Repository.Common;

namespace Workflow.Repository
{
    public class StepRepository : GenericRepository<Step>, IStepRepository
    {
        public StepRepository(IWorkflowDbContext context) : base(context)
        {
        }
    }
}