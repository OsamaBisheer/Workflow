using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class ActionTypeService : IActionTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ActionTypeService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var actionTypes = await unitOfWork.ActionTypes.Get(w => !w.IsDeleted).ToListAsync();
            return Tuple.Create(mapper.Map<List<ActionType>, List<LookupVM>>(actionTypes), ResponseCodeEnum.Success);
        }
    }
}