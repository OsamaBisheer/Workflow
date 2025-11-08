using System.ComponentModel.DataAnnotations;
using Workflow.Domain.ViewModels.Common;

namespace Workflow.Domain.ViewModels.Step
{
    public class StepCreateVM : AuditableVM
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string AssignedToRoleId { get; set; }

        public int ActionTypeId { get; set; }
        public string NextStepName { get; set; }
    }
}