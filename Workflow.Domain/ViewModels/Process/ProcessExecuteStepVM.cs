using Workflow.Domain.ViewModels.Common;

namespace Workflow.Domain.ViewModels.Process
{
    public class ProcessExecuteStepVM : AuditableVM
    {
        public int Id { get; set; }
        public string NextInitiatorId { get; set; }
        public int TargetStepId { get; set; }
        public bool ExternalAPIResult { get; set; }
    }
}