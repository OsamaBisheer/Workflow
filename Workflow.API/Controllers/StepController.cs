using Microsoft.AspNetCore.Mvc;
using Workflow.API.Controllers;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepController : CommonControllerBase
    {
        private readonly IStepService stepService;

        public StepController(IStepService _stepService)
        {
            stepService = _stepService;
        }

        [HttpGet, Route("lookup/{workflowId}")]
        public async Task<ActionResult> GetLookup(int workflowId)
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await stepService.GetLookup(workflowId);

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