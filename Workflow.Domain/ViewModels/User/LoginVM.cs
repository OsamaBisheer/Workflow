using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.ViewModels.User
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}