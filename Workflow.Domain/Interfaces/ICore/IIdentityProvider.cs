using Workflow.Domain.Entities.Identity;

namespace Workflow.Domain.Interfaces.ICore
{
    public interface IIdentityProvider
    {
        User GetUser();
    }
}