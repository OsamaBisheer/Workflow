using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class ProcessService : IProcessService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;

        public ProcessService(IUnitOfWork _unitOfWork, IMapper _mapper, HttpClient _httpClient)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            httpClient = _httpClient;
        }

        public async Task<Tuple<DataTableResponseVM<ProcessResultVM>, ResponseCodeEnum>> Search(ProcessDataTableRequestVM processVM)
        {
            var dataTableResponse = await unitOfWork.Processes.Search(processVM);
            return Tuple.Create(mapper.Map<DataTableResponseVM<Process>, DataTableResponseVM<ProcessResultVM>>(dataTableResponse), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Start(ProcessStartVM processVM)
        {
            var initActionTypeId = await unitOfWork.ActionTypes.Get(a => !a.IsDeleted && a.ActionTypeEnum == ActionTypeEnum.Initial).Select(a => a.Id).FirstOrDefaultAsync();

            if (initActionTypeId == 0) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            var targetStepId = await unitOfWork.Steps.Get(s => !s.IsDeleted && s.WorkflowId == processVM.WorkflowId && s.ActionTypeId == initActionTypeId).Select(s => s.Id).FirstOrDefaultAsync();

            if (targetStepId == 0) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            processVM.TargetStepId = targetStepId;
            var process = mapper.Map<ProcessStartVM, Process>(processVM);

            await unitOfWork.Processes.Add(process);

            await unitOfWork.Commit();

            return Tuple.Create(process.Id, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> ExecuteStep(ProcessExecuteStepVM processVM, string currentUserId)
        {
            var processDB = await unitOfWork.Processes.Get(p => p.Id == processVM.Id && !p.IsDeleted).AsNoTracking().FirstOrDefaultAsync();

            if (processDB == null) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            var status = await ExecuteStepBussinessValidations(processVM, currentUserId, processDB.CurrentStepId, processDB.CurrentInitiatorId);
            if (status != ResponseCodeEnum.Success) return Tuple.Create(0, status);

            var process = mapper.Map<ProcessExecuteStepVM, Process>(processVM);

            process.SetCreated(processDB.CreatedByUserId, processDB.CreatedOn);

            unitOfWork.Processes.Update(process);

            await unitOfWork.Commit();

            return Tuple.Create(process.Id, ResponseCodeEnum.Success);
        }

        private async Task<ResponseCodeEnum> ExecuteStepBussinessValidations(ProcessExecuteStepVM processVM, string currentUserId, int processCurrentStepId, string currentInitiatorId)
        {
            var status = ValidateSuitableInitaitor(currentUserId, currentInitiatorId);
            if (status != ResponseCodeEnum.Success) return status;

            var targetStep = await unitOfWork.Steps.Get(s => !s.IsDeleted && s.Id == processVM.TargetStepId)
                                    .Select(s => new Step { Name = s.Name, WorkflowId = s.WorkflowId, ActionTypeId = s.ActionTypeId })
                                    .FirstOrDefaultAsync();
            if (targetStep == null) return ResponseCodeEnum.NotFound;

            status = await ValidateInitaitorCanApplyStepAndPreviousSteps(processVM, currentUserId, processCurrentStepId, targetStep);
            if (status != ResponseCodeEnum.Success) return status;

            status = await ValidateShouldEnterInitiator(processVM, targetStep);
            if (status != ResponseCodeEnum.Success) return status;

            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateSuitableInitaitor(string currentUserId, string currentInitiatorId)
        {
            if (currentUserId != currentInitiatorId)
            {
                return ResponseCodeEnum.NotSuitableInitaitor;
            }

            return ResponseCodeEnum.Success;
        }

        private async Task<ResponseCodeEnum> ValidateInitaitorCanApplyStepAndPreviousSteps(ProcessExecuteStepVM processVM, string currentUserId, int processCurrentStepId, Step targetStep)
        {
            // should put in the repo
            var steps = await unitOfWork.Steps.Get(s => !s.IsDeleted && s.WorkflowId == targetStep.WorkflowId)
                                .Select(s => new Step
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    AssignedToRoleId = s.AssignedToRoleId,
                                    AssignedToRole = new ApplicationRole
                                    {
                                        Description = new ApplicationRoleDescription { ApplicationRoleDescriptionEnum = s.AssignedToRole.Description.ApplicationRoleDescriptionEnum }
                                                                            ,
                                        UserRoles = new List<ApplicationUserRole>()
                                    },
                                    ActionType = new ActionType { ActionTypeEnum = s.ActionType.ActionTypeEnum },
                                    NextStep = s.NextStep != null ? new Step { Name = s.NextStep.Name } : null
                                })
                                .ToDictionaryAsync(s => s.Name, s => s);

            if (steps == null) return ResponseCodeEnum.NotFound;

            var currentUserRoleIds = await unitOfWork.ApplicationUserRoles.Get(r => r.UserId == currentUserId)
                                    .Select(r => r.RoleId).ToListAsync();

            if (currentUserRoleIds == null) return ResponseCodeEnum.NotFound;

            var processCurrentStep = steps.Values.FirstOrDefault(s => s.Id == processCurrentStepId);

            if (processCurrentStep == null) return ResponseCodeEnum.NotFound;

            var currentStepInTheLoop = steps[processCurrentStep.NextStep.Name];

            while (currentStepInTheLoop != null)
            {
                if (currentStepInTheLoop.AssignedToRole.Description.ApplicationRoleDescriptionEnum != ApplicationRoleDescriptionEnum.System &&
                    !currentUserRoleIds.Contains(currentStepInTheLoop.AssignedToRoleId))
                    return ResponseCodeEnum.CantApplyStepAndPreviousSteps;

                if (currentStepInTheLoop.ActionType.ActionTypeEnum == ActionTypeEnum.ApprovalReject && !(await ValidateExternalAPI(processVM.ExternalAPIResult)))
                    return ResponseCodeEnum.ExternalAPINotAllowed;

                if (currentStepInTheLoop.Name == targetStep.Name)
                    break;

                if (currentStepInTheLoop.NextStep == null)
                    return ResponseCodeEnum.CantApplyStepAndPreviousSteps;

                currentStepInTheLoop = steps[currentStepInTheLoop.NextStep.Name];
            }

            return ResponseCodeEnum.Success;
        }

        private async Task<bool> ValidateExternalAPI(bool externalAPIResult)
        {
            var id = 1;
            var data = await httpClient.GetStringAsync($"https://jsonplaceholder.typicode.com/todos/{id}");
            dynamic result = JsonConvert.DeserializeObject(data);
            return externalAPIResult ? (result.id == id) : (result.id != id);
        }

        private async Task<ResponseCodeEnum> ValidateShouldEnterInitiator(ProcessExecuteStepVM processVM, Step targetStep)
        {
            var endActionTypeId = await unitOfWork.ActionTypes.Get(a => !a.IsDeleted && a.ActionTypeEnum == ActionTypeEnum.End)
                                    .Select(a => a.Id)
                                    .FirstOrDefaultAsync();

            if (endActionTypeId == 0) return ResponseCodeEnum.NotFound;

            if ((targetStep.ActionTypeId == endActionTypeId && !string.IsNullOrEmpty(processVM.NextInitiatorId)) ||
                (targetStep.ActionTypeId != endActionTypeId && string.IsNullOrEmpty(processVM.NextInitiatorId)))
            {
                return ResponseCodeEnum.ShouldEnterInitiatorViolation;
            }

            return ResponseCodeEnum.Success;
        }
    }
}