using System.ComponentModel.DataAnnotations;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Step;

namespace Workflow.Domain.ViewModels.Workflow
{
    public class WorkflowUpdateVM : AuditableVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<StepUpdateVM> Steps { get; set; }
    }
}