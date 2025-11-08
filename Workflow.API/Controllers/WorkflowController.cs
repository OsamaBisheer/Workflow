using Microsoft.AspNetCore.Mvc;
using Workflow.API.Controllers;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Workflow;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : CommonControllerBase
    {
        private readonly IWorkflowService workflowService;

        public WorkflowController(IWorkflowService _workflowService)
        {
            workflowService = _workflowService;
        }

        [HttpGet, Route("lookup")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await workflowService.GetLookup();

            return GetActionResult(new ResponseModel
            {
                Result = lookups,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("search")]
        public async Task<ActionResult> GetWorkflowsForPagination(DataTableRequestVM requestVM)
        {
            (DataTableResponseVM<WorkflowResultVM> dataTableResponseVM, ResponseCodeEnum responseCode) = await workflowService.Search(requestVM);

            return GetActionResult(new ResponseModel
            {
                Result = dataTableResponseVM,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult> Create(WorkflowCreateVM createVM)
        {
            if (createVM.Steps == null || createVM.Steps.Count == 0)
            {
                var missingStepsErrorMsg = "Model should have one step at least";
                ModelState.AddModelError("Steps", missingStepsErrorMsg);
            }

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

            createVM.CreatedByUserId = GetCurrentUserId();
            (int id, ResponseCodeEnum responseCode) = await workflowService.Create(createVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        //Need Review
        //[HttpPut, Route("update")]
        //public async Task<ActionResult> Update(WorkflowUpdateVM updateVM)
        //{
        //    if (updateVM.Steps == null || updateVM.Steps.Count == 0)
        //    {
        //        var missingStepsErrorMsg = "Model should have one step at least";
        //        ModelState.AddModelError("Steps", missingStepsErrorMsg);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return GetActionResult(new ResponseModel
        //        {
        //            Result = 0,
        //            Code = ResponseCodeEnum.BadRequest,
        //            MessageFL = nameof(ResponseCodeEnum.BadRequest),
        //            MessageSL = nameof(ResponseCodeEnum.BadRequest)
        //        });
        //    }

        //    (int id, ResponseCodeEnum responseCode) = await workflowService.Update(updateVM);

        //    return GetActionResult(new ResponseModel
        //    {
        //        Result = id,
        //        Code = responseCode,
        //        MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
        //        MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
        //    });
        //}
    }
}