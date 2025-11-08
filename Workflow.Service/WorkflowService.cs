using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Step;
using Workflow.Domain.ViewModels.Workflow;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public WorkflowService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var workflows = await unitOfWork.Workflows.Get(w => !w.IsDeleted).ToListAsync();
            return Tuple.Create(mapper.Map<List<Workflow.Domain.Entities.Workflow>, List<LookupVM>>(workflows), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<DataTableResponseVM<WorkflowResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM requestVM)
        {
            var dataTableResponse = await unitOfWork.Workflows.Search(requestVM);
            return Tuple.Create(mapper.Map<DataTableResponseVM<Workflow.Domain.Entities.Workflow>, DataTableResponseVM<WorkflowResultVM>>(dataTableResponse), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Create(WorkflowCreateVM workflowVM)
        {
            var status = CreateUpdateBussinessValidations(workflowVM.Steps.Select(s => s.NextStepName).ToList(),
                                                    workflowVM.Steps.Select(s => s.NextStepName).ToList(),
                                                    workflowVM.Steps.ToDictionary(s => s.Name, s => s.NextStepName)
                                                    );
            if (status != ResponseCodeEnum.Success) return Tuple.Create(0, status);

            workflowVM.Steps = await AddSystemMandatorySteps(workflowVM.Steps);

            var workflow = mapper.Map<WorkflowCreateVM, Workflow.Domain.Entities.Workflow>(workflowVM);
            foreach (var step in workflowVM.Steps.Where(s => !string.IsNullOrEmpty(s.NextStepName)).ToList())
            {
                var targetStep = workflow.Steps.SingleOrDefault(s => s.Name == step.Name);
                var targetNextStep = workflow.Steps.SingleOrDefault(s => s.Name == step.NextStepName);
                targetStep.NextStep = targetNextStep;
            }

            await unitOfWork.Workflows.Add(workflow);

            await unitOfWork.Commit();

            return Tuple.Create(workflow.Id, ResponseCodeEnum.Success);

            //Unsure         dont forget mapper profiles, Set updating and creating in mappers and other primiary fields correctly
            //Done          In process, check valid initiator role when assign initator, check it when execute step , check if the executer is the same for this excute step and all previous non executed, (reach final step like others, no initiator for it, mustn't inititator for it and must for others?
            //In workflow controller , check Steps Required and should be one at least - should use if (!ModelState.IsValid) return BadRequest(ModelState) for all create and update;
        }

        private async Task<List<StepCreateVM>> AddSystemMandatorySteps(List<StepCreateVM> steps)
        {
            var targetedStepsActionTypes = await unitOfWork.ActionTypes.Get(t => !t.IsDeleted && (t.ActionTypeEnum == ActionTypeEnum.Initial || t.ActionTypeEnum == ActionTypeEnum.End))
                                                .Select(t => new ActionType { Id = t.Id, ActionTypeEnum = t.ActionTypeEnum }).ToListAsync();

            var systemRoleId = await unitOfWork.ApplicationRoleDescriptions.Get(d => !d.IsDeleted && d.ApplicationRoleDescriptionEnum == ApplicationRoleDescriptionEnum.System)
                                                .Select(d => d.Id).FirstOrDefaultAsync();

            var lastWorkflowIdInDB = await unitOfWork.Workflows.Get(w => !w.IsDeleted)
                                            .OrderByDescending(w => w.Id).Select(w => w.Id).FirstOrDefaultAsync();

            var nextStepNames = steps.Select(s => s.NextStepName).ToList();

            var firstStepFromUser = steps.Single(s => !nextStepNames.Contains(s.Name));

            var initialStep = new StepCreateVM
            {
                ActionTypeId = targetedStepsActionTypes.FirstOrDefault(t => t.ActionTypeEnum == ActionTypeEnum.Initial).Id,
                AssignedToRoleId = systemRoleId,
                Name = nameof(ActionTypeEnum.Initial) + (lastWorkflowIdInDB + 1),
                NextStepName = firstStepFromUser.Name
            };

            steps = steps.Prepend(initialStep).ToList();

            var finalStepFromUser = steps.Single(s => string.IsNullOrEmpty(s.NextStepName));

            var endStep = new StepCreateVM
            {
                ActionTypeId = targetedStepsActionTypes.FirstOrDefault(t => t.ActionTypeEnum == ActionTypeEnum.End).Id,
                AssignedToRoleId = systemRoleId,
                Name = nameof(ActionTypeEnum.End) + (lastWorkflowIdInDB + 1)
            };

            steps.Add(endStep);

            finalStepFromUser.NextStepName = nameof(ActionTypeEnum.End) + (lastWorkflowIdInDB + 1);

            return steps;
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Update(WorkflowUpdateVM workflowVM)
        {
            var status = CreateUpdateBussinessValidations(workflowVM.Steps.Select(s => s.NextStepName).ToList(),
                                                    workflowVM.Steps.Select(s => s.NextStepName).ToList(),
                                                    workflowVM.Steps.ToDictionary(s => s.Name, s => s.NextStepName)
                                                    );

            if (status != ResponseCodeEnum.Success) return Tuple.Create(0, status);

            var workflowDB = await unitOfWork.Workflows.Find(workflowVM.Id);

            if (workflowDB == null || workflowDB.IsDeleted) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            var workflow = mapper.Map<WorkflowUpdateVM, Workflow.Domain.Entities.Workflow>(workflowVM);
            foreach (var step in workflowVM.Steps.Where(s => !string.IsNullOrEmpty(s.NextStepName)).ToList())
            {
                var targetStep = workflow.Steps.SingleOrDefault(s => s.Name == step.Name);
                var targetNextStep = workflow.Steps.SingleOrDefault(s => s.Name == step.NextStepName);
                targetStep.NextStep = targetNextStep;
            }

            workflow.SetCreated(workflowDB.CreatedByUserId, workflowDB.CreatedOn);

            unitOfWork.Workflows.Update(workflow);

            await unitOfWork.Commit();

            return Tuple.Create(workflowDB.Id, ResponseCodeEnum.Success);
        }

        private ResponseCodeEnum CreateUpdateBussinessValidations(List<string> stepNameList, List<string> nextStepNameList, Dictionary<string, string> stepAndNextStepNameDictionary)
        {
            var status = ValidateDuplication(stepNameList);
            if (status != ResponseCodeEnum.Success) return status;

            status = ValidateDuplication(nextStepNameList);
            if (status != ResponseCodeEnum.Success) return status;

            status = ValidateNextStepNameAtLeastOneNullAndOneOnly(nextStepNameList);
            if (status != ResponseCodeEnum.Success) return status;

            status = ValidateStepNameNotEqualStepNextStepName(stepAndNextStepNameDictionary);
            if (status != ResponseCodeEnum.Success) return status;

            status = ValidateNamesNotSystemStepsNames(stepNameList);
            if (status != ResponseCodeEnum.Success) return status;

            status = ValidateNextStepsNamesReferToStepsNames(stepNameList, nextStepNameList);
            if (status != ResponseCodeEnum.Success) return status;

            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateDuplication(List<string> names)
        {
            if (names.Count() != names.Distinct().Count()) return ResponseCodeEnum.Duplicate;
            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateNextStepNameAtLeastOneNullAndOneOnly(List<string> names)
        {
            if (names.Where(n => string.IsNullOrEmpty(n)).Count() != 1) return ResponseCodeEnum.NextStepNameOneNullAndOneOnlyViolated;
            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateStepNameNotEqualStepNextStepName(Dictionary<string, string> keyValuePairs)
        {
            if (keyValuePairs.Any(kv => kv.Key == kv.Value)) return ResponseCodeEnum.NextStepReferToTheSameStep;
            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateNamesNotSystemStepsNames(List<string> names)
        {
            if (names.Any(n => n == nameof(ActionTypeEnum.Initial) || n == nameof(ActionTypeEnum.End))) return ResponseCodeEnum.NamesAreSystemStepsNames;
            return ResponseCodeEnum.Success;
        }

        private ResponseCodeEnum ValidateNextStepsNamesReferToStepsNames(List<string> names, List<string> nextStepsNames)
        {
            if (nextStepsNames.Any(n => !names.Contains(n))) return ResponseCodeEnum.NextStepNotReferToStepsNames;
            return ResponseCodeEnum.Success;
        }
    }
}