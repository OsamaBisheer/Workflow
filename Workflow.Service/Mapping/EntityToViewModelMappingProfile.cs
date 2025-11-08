using AutoMapper;
using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.ViewModels.Common;
using Workflow.Domain.ViewModels.Process;
using Workflow.Domain.ViewModels.Step;
using Workflow.Domain.ViewModels.Workflow;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Service.Mappings
{
    public class EntityToViewModelMappingProfile : Profile
    {
        public EntityToViewModelMappingProfile()
        {
            CreateMap<Workflow.Domain.Entities.Workflow, LookupVM>();
            CreateMap<DataTableResponseVM<Workflow.Domain.Entities.Workflow>, DataTableResponseVM<WorkflowResultVM>>();
            CreateMap<Workflow.Domain.Entities.Workflow, WorkflowResultVM>();
            CreateMap<Step, StepResultVM>()
                .ForMember(Dest => Dest.AssignedToRoleNameFL, opt => opt.MapFrom(src => src.AssignedToRole.Description.NameFL))
                .ForMember(Dest => Dest.AssignedToRoleNameSL, opt => opt.MapFrom(src => src.AssignedToRole.Description.NameSL))
                .ForMember(Dest => Dest.AssignedToRoleEnum, opt => opt.MapFrom(src => src.AssignedToRole.Description.ApplicationRoleDescriptionEnum))
                .ForMember(Dest => Dest.ActionTypeNameFL, opt => opt.MapFrom(src => src.ActionType.NameFL))
                .ForMember(Dest => Dest.ActionTypeNameSL, opt => opt.MapFrom(src => src.ActionType.NameSL))
                .ForMember(Dest => Dest.ActionTypeEnum, opt => opt.MapFrom(src => src.ActionType.ActionTypeEnum))
                .ForMember(Dest => Dest.NextStepName, opt => opt.MapFrom(src => src.NextStep.Name));

            CreateMap<ActionType, LookupVM>()
                .ForMember(Dest => Dest.Name, opt => opt.MapFrom(src => src.NameFL));

            CreateMap<ApplicationRoleDescription, LookupVM>()
                .ForMember(Dest => Dest.Name, opt => opt.MapFrom(src => src.NameFL));

            CreateMap<Step, LookupVM>();

            CreateMap<User, LookupVM>()
                .ForMember(Dest => Dest.Name, opt => opt.MapFrom(src => src.UserName));

            CreateMap<DataTableResponseVM<Process>, DataTableResponseVM<ProcessResultVM>>();
            CreateMap<Process, ProcessResultVM>()
                .ForMember(Dest => Dest.WorkflowName, opt => opt.MapFrom(src => src.CurrentStep.Workflow.Name))
                .ForMember(Dest => Dest.Status, opt => opt.MapFrom(src =>
                    src.CurrentStep.ActionType.ActionTypeEnum == ActionTypeEnum.Initial ? ProcessStatusEnum.Pending :
                    src.CurrentStep.ActionType.ActionTypeEnum == ActionTypeEnum.End ? ProcessStatusEnum.Completed :
                    ProcessStatusEnum.Active))
                .ForMember(Dest => Dest.CurrentInitiatorName, opt => opt.MapFrom(src => src.CurrentInitiator.UserName))
                .ForMember(Dest => Dest.CurrentStepName, opt => opt.MapFrom(src => src.CurrentStep.Name));
        }
    }
}