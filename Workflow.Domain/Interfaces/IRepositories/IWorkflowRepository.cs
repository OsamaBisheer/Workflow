using Workflow.Domain.ViewModels.Common;

namespace Workflow.Domain.Interfaces.IRepositories
{
    public interface IWorkflowRepository : IGenericRepository<Entities.Workflow>
    {
        Task<DataTableResponseVM<Entities.Workflow>> Search(DataTableRequestVM workflowVM);
    }
}