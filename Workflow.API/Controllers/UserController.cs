using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Workflow.API.Controllers;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.User;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : CommonControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IConfiguration config;
        private readonly IIdentityProvider identityProvider;

        public UserController(IUserService _userService, UserManager<User> _userManager, SignInManager<User> _signInManager, IConfiguration _config, IIdentityProvider _identityProvider)
        {
            userService = _userService;
            userManager = _userManager;
            signInManager = _signInManager;
            config = _config;
            identityProvider = _identityProvider;
        }

        [HttpGet, Route("lookup")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await userService.GetLookup();

            return GetActionResult(new ResponseModel
            {
                Result = lookups,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var signInResult = signInManager.PasswordSignInAsync(user, model.Password, true, false).Result;
            if (!signInResult.Succeeded) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id),
            };

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(config["Jwt:ExpiresInHours"])),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return GetActionResult(new ResponseModel
            {
                Result = tokenString,
                Code = ResponseCodeEnum.Success
            });
        }

        [HttpGet("Logout")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            var user = identityProvider.GetUser();
            if (user != null)
            {
                await signInManager.SignOutAsync();
            }
            return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.Success
            });
        }

        [HttpPost("AddUser")]
        [AllowAnonymous]
        public async Task<ActionResult> AddUser(UserAddVM model)
        {
            if (model.RolesIds == null || model.RolesIds.Count == 0)
            {
                var missingRolesErrorMsg = "Model should have one role at least";
                ModelState.AddModelError("RolesIds", missingRolesErrorMsg);
            }
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.BadRequest,
                MessageFL = nameof(ResponseCodeEnum.BadRequest),
                MessageSL = nameof(ResponseCodeEnum.BadRequest)
            });

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.InternalServerError,
                MessageFL = nameof(ResponseCodeEnum.InternalServerError),
                MessageSL = nameof(ResponseCodeEnum.InternalServerError)
            });

            var resultCode = await userService.AddUserRoles(user.Id, model.RolesIds);
            return GetActionResult(new ResponseModel
            {
                Code = resultCode,
                MessageFL = resultCode == ResponseCodeEnum.Success ? null : resultCode.ToString(),
                MessageSL = resultCode == ResponseCodeEnum.Success ? null : resultCode.ToString()
            });
        }
    }
}