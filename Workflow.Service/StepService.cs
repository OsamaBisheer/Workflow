using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class StepService : IStepService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StepService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup(int workflowId)
        {
            var steps = await unitOfWork.Steps.Get(w => !w.IsDeleted && w.WorkflowId == workflowId).ToListAsync();
            return Tuple.Create(mapper.Map<List<Step>, List<LookupVM>>(steps), ResponseCodeEnum.Success);
        }
    }
}