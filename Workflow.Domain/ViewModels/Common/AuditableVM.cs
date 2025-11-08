namespace Workflow.Domain.ViewModels.Common
{
    public class AuditableVM
    {
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}