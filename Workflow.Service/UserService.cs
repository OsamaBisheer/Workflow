using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Domain.ViewModels.Common;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var users = await unitOfWork.Users.GetAll().ToListAsync();
            return Tuple.Create(mapper.Map<List<User>, List<LookupVM>>(users), ResponseCodeEnum.Success);
        }

        public async Task<ResponseCodeEnum> AddUserRoles(string userId, List<string> rolesIds)
        {
            var userRoles = rolesIds.Select(id => new ApplicationUserRole { UserId = userId, RoleId = id });
            await unitOfWork.ApplicationUserRoles.AddRange(userRoles);
            await unitOfWork.Commit();

            return ResponseCodeEnum.Success;
        }
    }
}