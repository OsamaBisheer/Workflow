using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Domain.Entities.Identity;

namespace Workflow.Domain.Entities.Common
{
    public class AuditableEntity : Entity
    {
        public AuditableEntity()
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            CreatedOn = dateTimeNow;
        }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string LastUpdatedByUserId { get; set; }

        [ForeignKey("LastUpdatedByUserId")]
        public User LastUpdatedByUser { get; set; }

        public DateTime? LastUpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public void SetCreated(string createdByUserId, DateTime? createdOn)
        {
            if (!string.IsNullOrEmpty(createdByUserId)) CreatedByUserId = createdByUserId;
            if (createdOn != null) CreatedOn = createdOn;
        }

        public void SetLastUpdated(string lastUpdatedByUserId, DateTime? lastUpdatedOn)
        {
            if (!string.IsNullOrEmpty(lastUpdatedByUserId)) LastUpdatedByUserId = lastUpdatedByUserId;
            if (lastUpdatedOn != null) LastUpdatedOn = lastUpdatedOn;
        }

        public void MarkAsDeleted(string lastUpdatedByUserId)
        {
            IsDeleted = true;
            LastUpdatedByUserId = lastUpdatedByUserId;
            LastUpdatedOn = DateTime.UtcNow;
        }
    }
}