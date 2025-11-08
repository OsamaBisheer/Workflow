using Microsoft.AspNetCore.Mvc;
using Workflow.API.Controllers;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationRoleDescriptionController : CommonControllerBase
    {
        private readonly IApplicationRoleDescriptionService applicationRoleDescriptionService;

        public ApplicationRoleDescriptionController(IApplicationRoleDescriptionService _applicationRoleDescriptionService)
        {
            applicationRoleDescriptionService = _applicationRoleDescriptionService;
        }

        [HttpGet, Route("lookup")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await applicationRoleDescriptionService.GetLookup();

            return GetActionResult(new ResponseModel
            {
                Result = lookups,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }
    }
}