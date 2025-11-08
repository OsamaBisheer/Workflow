using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Workflow;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Interfaces.IServices
{
    public interface IWorkflowService
    {
        Task<Tuple<int, ResponseCodeEnum>> Create(WorkflowCreateVM workflowVM);

        Task<Tuple<int, ResponseCodeEnum>> Update(WorkflowUpdateVM workflowVM);

        Task<Tuple<DataTableResponseVM<WorkflowResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM workflowVM);

        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup();
    }
}