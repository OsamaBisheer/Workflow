using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API.Controllers
{
    [ApiController]
    [Authorize]
    public class CommonControllerBase : ControllerBase
    {
        protected string GetCurrentUserId()
        {
            try
            {
                return GetClaimValue(new IdentityOptions().ClaimsIdentity.UserIdClaimType);
            }
            catch
            {
                return null;
            }
        }

        protected string GetCurrentUserName()
        {
            try
            {
                return GetClaimValue(new IdentityOptions().ClaimsIdentity.UserNameClaimType);
            }
            catch
            {
                return null;
            }
        }

        protected string GetClaimValue(string claimType)
        {
            string resolvedID = null;
            try
            {
                resolvedID = User.Claims.First(i => i.Type == claimType).Value;
            }
            catch
            {
                resolvedID = null;
            }

            return resolvedID;
        }

        protected ActionResult GetActionResult(ResponseModel responseModel)
        {
            return responseModel.Code switch
            {
                ResponseCodeEnum.Success => Ok(responseModel),
                ResponseCodeEnum.UnAuthorized => Unauthorized(responseModel),
                ResponseCodeEnum.Forbidden => Forbid(),
                ResponseCodeEnum.NotFound => NotFound(responseModel),
                ResponseCodeEnum.MethodNotAllowed => StatusCode(405),
                ResponseCodeEnum.Duplicate => StatusCode(409),
                ResponseCodeEnum.InternalServerError => StatusCode(500),
                _ => BadRequest(responseModel)
            };
        }
    }
}