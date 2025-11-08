using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Domain.ViewModels.Common;
using Workflow.Repository.Common;
using Workflow.Repository.Helpers;

namespace Workflow.Repository
{
    public class WorkflowRepository : GenericRepository<Workflow.Domain.Entities.Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(IWorkflowDbContext context) : base(context)
        {
        }

        public async Task<DataTableResponseVM<Workflow.Domain.Entities.Workflow>> Search(DataTableRequestVM requestVM)
        {
            var search = string.IsNullOrEmpty(requestVM.Search) ? string.Empty : requestVM.Search.ToLower();

            var result = GetAll().OrderByDescending(w => w.Id).Where(w => !w.IsDeleted)
                .Where(w => w.Id.ToString() == search || w.Name.ToLower().Contains(search))
                .Select(w => new Workflow.Domain.Entities.Workflow
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    Steps = w.Steps.Select(s => new Step
                    {
                        Id = s.Id,
                        Name = s.Name,
                        AssignedToRole = new ApplicationRole
                        {
                            Description = new ApplicationRoleDescription
                            {
                                NameFL = s.AssignedToRole.Description.NameFL,
                                NameSL = s.AssignedToRole.Description.NameSL,
                                ApplicationRoleDescriptionEnum = s.AssignedToRole.Description.ApplicationRoleDescriptionEnum
                            },
                            UserRoles = new List<ApplicationUserRole>()
                        },
                        ActionType = new ActionType
                        {
                            NameFL = s.ActionType.NameFL,
                            NameSL = s.ActionType.NameSL,
                            ActionTypeEnum = s.ActionType.ActionTypeEnum
                        },
                        NextStep = new Step
                        {
                            Name = s.NextStepId == null ? null : s.NextStep.Name,
                        }
                    }).ToList()
                });

            int totalRecords = result.Count();

            result = result.OrderByDynamic(requestVM.OrderColumn, requestVM.OrderDir)
                .Skip(requestVM.PageNumber * requestVM.PageSize)
                .Take(requestVM.PageSize);

            return await result.ToDataTableResult(totalRecords);
        }
    }
}