using AutoMapper;
using Workflow.Domain.Entities;
using Workflow.Domain.ViewModels.Process;
using Workflow.Domain.ViewModels.Step;
using Workflow.Domain.ViewModels.Workflow;

namespace Workflow.Service.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<WorkflowCreateVM, Workflow.Domain.Entities.Workflow>().AfterMap((vm, entity) => entity.SetCreated(entity.CreatedByUserId, DateTime.UtcNow));
            CreateMap<StepCreateVM, Step>().AfterMap((vm, entity) => entity.SetCreated(entity.CreatedByUserId, DateTime.UtcNow));
            //CreateMap<WorkflowUpdateVM, Workflow.Domain.Entities.Workflow>().AfterMap((vm, entity) => entity.SetLastUpdated(entity.LastUpdatedByUserId, DateTime.UtcNow));
            //CreateMap<StepUpdateVM, Step>().AfterMap((vm, entity) => entity.SetLastUpdated(entity.LastUpdatedByUserId, DateTime.UtcNow));

            CreateMap<ProcessStartVM, Process>()
                .ForMember(Dest => Dest.CurrentInitiatorId, opt => opt.MapFrom(src => src.NextInitiatorId))
                .ForMember(Dest => Dest.CurrentStepId, opt => opt.MapFrom(src => src.TargetStepId))
                .AfterMap((vm, entity) => entity.SetCreated(entity.CreatedByUserId, DateTime.UtcNow));
            CreateMap<ProcessExecuteStepVM, Process>()
                .ForMember(Dest => Dest.CurrentInitiatorId, opt => opt.MapFrom(src => src.NextInitiatorId))
                .ForMember(Dest => Dest.CurrentStepId, opt => opt.MapFrom(src => src.TargetStepId))
                .AfterMap((vm, entity) => entity.SetLastUpdated(entity.LastUpdatedByUserId, DateTime.UtcNow));
        }
    }
}