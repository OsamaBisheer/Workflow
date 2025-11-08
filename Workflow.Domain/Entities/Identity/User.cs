using Microsoft.AspNetCore.Identity;

namespace Workflow.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public List<ApplicationUserRole> UserRoles { get; set; }
        public List<Process> InitatorProcesses { get; set; }
    }
}