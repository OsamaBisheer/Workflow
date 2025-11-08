using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Domain.Entities.Common;
using Workflow.Domain.Entities.Identity;

namespace Workflow.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(WorkflowId), nameof(NextStepId), IsUnique = true)]
    public class Step : AuditableEntity
    {
        public string Name { get; set; }
        public string AssignedToRoleId { get; set; }

        [ForeignKey("AssignedToRoleId")]
        public ApplicationRole AssignedToRole { get; set; }

        public int ActionTypeId { get; set; }

        [ForeignKey("ActionTypeId")]
        public ActionType ActionType { get; set; }

        public int WorkflowId { get; set; }

        [ForeignKey("WorkflowId")]
        public Workflow Workflow { get; set; }

        public int? NextStepId { get; set; }

        [ForeignKey("NextStepId")]
        public Step NextStep { get; set; }

        public List<Process> Processes { get; set; }
    }
}