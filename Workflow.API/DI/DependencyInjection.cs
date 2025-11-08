using Workflow.API.JWT;
using Workflow.API.Providers;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;
using Workflow.Domain.Interfaces.IServices;
using Workflow.Persistence;
using Workflow.Repository;
using Workflow.Repository.Common;
using Workflow.Service;

namespace Workflow.API.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDIs(this IServiceCollection services)
        {
            services.AddScoped<IWorkflowDbContext, WorkflowDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityProvider, IdentityProvider>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IApplicationRoleDescriptionRepository, ApplicationRoleDescriptionRepository>();
            services.AddScoped<IApplicationUserRoleRepository, ApplicationUserRoleRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
            services.AddScoped<IStepRepository, StepRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();

            services.AddScoped<IActionTypeService, ActionTypeService>();
            services.AddScoped<IApplicationRoleDescriptionService, ApplicationRoleDescriptionService>();
            services.AddHttpClient<ProcessService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IStepService, StepService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorkflowService, WorkflowService>();

            services.AddScoped<RevokableJwtSecurityTokenHandler>();
            services.AddScoped<JwtHandlerEvents>();

            return services;
        }
    }
}