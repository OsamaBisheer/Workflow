using System.ComponentModel.DataAnnotations;
using Workflow.Domain.ViewModels.Common;

namespace Workflow.Domain.ViewModels.Process
{
    public class ProcessStartVM : AuditableVM
    {
        [Required]
        public string NextInitiatorId { get; set; }

        public int WorkflowId { get; set; }

        //Set to init by code
        public int TargetStepId { get; set; }
    }
}