using Workflow.Domain.Entities;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;

namespace Workflow.Domain.Interfaces.IRepositories
{
    public interface IProcessRepository : IGenericRepository<Process>
    {
        Task<DataTableResponseVM<Process>> Search(ProcessDataTableRequestVM processVM);
    }
}