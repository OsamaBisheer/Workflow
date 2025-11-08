using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Domain.Entities.Common;
using Workflow.Domain.Entities.Identity;

namespace Workflow.Domain.Entities
{
    public class Process : AuditableEntity
    {
        public string CurrentInitiatorId { get; set; }

        [ForeignKey("CurrentInitiatorId")]
        public User CurrentInitiator { get; set; }

        public int CurrentStepId { get; set; }

        [ForeignKey("CurrentStepId")]
        public Step CurrentStep { get; set; }
    }
}