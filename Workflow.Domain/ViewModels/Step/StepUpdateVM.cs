using System.ComponentModel.DataAnnotations;
using Workflow.Domain.ViewModels.Common;

namespace Workflow.Domain.ViewModels.Step
{
    public class StepUpdateVM : AuditableVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string AssignedToRoleId { get; set; }

        public int ActionTypeId { get; set; }
        public string NextStepName { get; set; }
    }
}