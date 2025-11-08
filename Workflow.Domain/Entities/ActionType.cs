using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.Domain.Entities.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Entities
{
    [Index(nameof(ActionTypeEnum), IsUnique = true)]
    public class ActionType : AuditableEntity
    {
        public string NameFL { get; set; }
        public string NameSL { get; set; }
        public List<Step> Steps { get; set; }

        [Column(TypeName = "tinyint")]
        public ActionTypeEnum ActionTypeEnum { get; set; }
    }
}