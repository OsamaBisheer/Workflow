using Microsoft.AspNetCore.Mvc;
using Workflow.API.Controllers;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActionTypeController : CommonControllerBase
    {
        private readonly IActionTypeService actionTypeService;

        public ActionTypeController(IActionTypeService _actionTypeService)
        {
            actionTypeService = _actionTypeService;
        }

        [HttpGet, Route("lookup")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await actionTypeService.GetLookup();

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