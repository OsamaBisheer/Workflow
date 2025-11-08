using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class ApplicationRoleDescriptionService : IApplicationRoleDescriptionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ApplicationRoleDescriptionService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var applicationRoleDescriptions = await unitOfWork.ApplicationRoleDescriptions.Get(w => !w.IsDeleted).ToListAsync();
            return Tuple.Create(mapper.Map<List<ApplicationRoleDescription>, List<LookupVM>>(applicationRoleDescriptions), ResponseCodeEnum.Success);
        }
    }
}