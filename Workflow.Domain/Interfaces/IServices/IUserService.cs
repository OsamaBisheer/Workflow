using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Domain.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup();

        Task<ResponseCodeEnum> AddUserRoles(string userId, List<string> rolesIds);
    }
}