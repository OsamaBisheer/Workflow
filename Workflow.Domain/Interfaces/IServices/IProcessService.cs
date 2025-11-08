using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Interfaces.IServices
{
    public interface IProcessService
    {
        Task<Tuple<int, ResponseCodeEnum>> Start(ProcessStartVM processVM);

        Task<Tuple<int, ResponseCodeEnum>> ExecuteStep(ProcessExecuteStepVM processVM, string currentUserId);

        Task<Tuple<DataTableResponseVM<ProcessResultVM>, ResponseCodeEnum>> Search(ProcessDataTableRequestVM processVM);
    }
}