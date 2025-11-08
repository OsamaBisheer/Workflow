using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.ViewModels.Process
{
    public class ProcessDataTableRequestVM : DataTableRequestVM
    {
        public int? WorkflowId { get; set; }
        public ProcessStatusEnum? Status { get; set; }
        public string CurrentInitiatorId { get; set; }
    }
}