using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;

namespace Workflow.API.Providers
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _httpCtxAccessor;
        private readonly UserManager<User> _userMgr;

        public IdentityProvider(IHttpContextAccessor httpCtxAccessor, UserManager<User> userMgr)
        {
            _httpCtxAccessor = httpCtxAccessor;
            _userMgr = userMgr;
        }

        public User GetUser()
        {
            try
            {
                if (_httpCtxAccessor.HttpContext.User.Identity.Name != null)
                {
                    var userName = _httpCtxAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name)
                        .Select(c => c.Value).FirstOrDefault();
                    var user = _userMgr.FindByNameAsync(userName).Result;
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}