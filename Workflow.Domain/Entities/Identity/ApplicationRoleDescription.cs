using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Domain.Entities.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Entities.Identity
{
    [Index(nameof(ApplicationRoleDescriptionEnum), IsUnique = true)]
    public class ApplicationRoleDescription : AuditableEntity
    {
        [Key]
        public new string Id { get; set; }

        public string NameFL { get; set; }
        public string NameSL { get; set; }

        [ForeignKey("Id")]
        public ApplicationRole ApplicationRole { get; set; }

        [Column(TypeName = "tinyint")]
        public ApplicationRoleDescriptionEnum ApplicationRoleDescriptionEnum { get; set; }
    }
}