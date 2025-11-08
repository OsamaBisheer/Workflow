using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities.Common;

namespace Workflow.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Workflow : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Step> Steps { get; set; }
    }
}