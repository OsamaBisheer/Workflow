using Microsoft.AspNetCore.Identity;

namespace Workflow.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRoleDescription Description { get; set; }
        public List<ApplicationUserRole> UserRoles { get; set; }
        public List<Step> Steps { get; set; }
    }
}