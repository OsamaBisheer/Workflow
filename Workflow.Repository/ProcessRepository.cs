using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;
using Workflow.Repository.Common;
using Workflow.Repository.Helpers;

namespace Workflow.Repository
{
    public class ProcessRepository : GenericRepository<Process>, IProcessRepository
    {
        public ProcessRepository(IWorkflowDbContext context) : base(context)
        {
        }

        public async Task<DataTableResponseVM<Process>> Search(ProcessDataTableRequestVM requestVM)
        {
            var search = string.IsNullOrEmpty(requestVM.Search) ? string.Empty : requestVM.Search.ToLower();

            var result = GetAll().OrderByDescending(p => p.Id).Where(p => !p.IsDeleted)
                .Where(p => string.IsNullOrEmpty(search) || p.Id.ToString() == search)
                .Where(p => requestVM.WorkflowId == null || p.CurrentStep.WorkflowId == requestVM.WorkflowId)
                .Where(p => requestVM.Status == null ||
                    (requestVM.Status == Domain.Enums.Enumeration.ProcessStatusEnum.Pending &&
                        p.CurrentStep.ActionType.ActionTypeEnum == Domain.Enums.Enumeration.ActionTypeEnum.Initial) ||
                    (requestVM.Status == Domain.Enums.Enumeration.ProcessStatusEnum.Active &&
                        p.CurrentStep.ActionType.ActionTypeEnum != Domain.Enums.Enumeration.ActionTypeEnum.Initial &&
                        p.CurrentStep.ActionType.ActionTypeEnum != Domain.Enums.Enumeration.ActionTypeEnum.End) ||
                    (requestVM.Status == Domain.Enums.Enumeration.ProcessStatusEnum.Completed &&
                        p.CurrentStep.ActionType.ActionTypeEnum == Domain.Enums.Enumeration.ActionTypeEnum.End)
                )
                .Where(p => string.IsNullOrEmpty(requestVM.CurrentInitiatorId) || p.CurrentInitiatorId == requestVM.CurrentInitiatorId)
                .Select(p => new Process
                {
                    Id = p.Id,
                    CurrentStep = new Step
                    {
                        Workflow = new Domain.Entities.Workflow
                        {
                            Name = p.CurrentStep.Workflow.Name
                        },
                        Name = p.CurrentStep.Name,
                        ActionType = new ActionType
                        {
                            ActionTypeEnum = p.CurrentStep.ActionType.ActionTypeEnum
                        }
                    },
                    CurrentInitiator = new User
                    {
                        UserName = p.CurrentInitiator.UserName,
                        UserRoles = new List<ApplicationUserRole>()
                    }
                });

            int totalRecords = result.Count();

            result = result.OrderByDynamic(requestVM.OrderColumn, requestVM.OrderDir)
                .Skip(requestVM.PageNumber * requestVM.PageSize)
                .Take(requestVM.PageSize);

            return await result.ToDataTableResult(totalRecords);
        }
    }
}