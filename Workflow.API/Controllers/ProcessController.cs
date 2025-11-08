using Microsoft.AspNetCore.Mvc;
using Workflow.API.Controllers;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : CommonControllerBase
    {
        private readonly IProcessService processService;

        public ProcessController(IProcessService _processService)
        {
            processService = _processService;
        }

        [HttpPost, Route("search")]
        public async Task<ActionResult> GetProcessesForPagination(ProcessDataTableRequestVM requestVM)
        {
            (DataTableResponseVM<ProcessResultVM> dataTableResponseVM, ResponseCodeEnum responseCode) = await processService.Search(requestVM);

            return GetActionResult(new ResponseModel
            {
                Result = dataTableResponseVM,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("start")]
        public async Task<ActionResult> Create(ProcessStartVM processVM)
        {
            if (!ModelState.IsValid)
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.BadRequest,
                    MessageFL = nameof(ResponseCodeEnum.BadRequest),
                    MessageSL = nameof(ResponseCodeEnum.BadRequest)
                });
            }

            processVM.CreatedByUserId = GetCurrentUserId();
            (int id, ResponseCodeEnum responseCode) = await processService.Start(processVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPut, Route("execute-step")]
        public async Task<ActionResult> Update(ProcessExecuteStepVM processVM)
        {
            if (!ModelState.IsValid)
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.BadRequest,
                    MessageFL = nameof(ResponseCodeEnum.BadRequest),
                    MessageSL = nameof(ResponseCodeEnum.BadRequest)
                });
            }

            processVM.LastUpdatedByUserId = GetCurrentUserId();
            (int id, ResponseCodeEnum responseCode) = await processService.ExecuteStep(processVM, GetCurrentUserId());

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }
    }
}