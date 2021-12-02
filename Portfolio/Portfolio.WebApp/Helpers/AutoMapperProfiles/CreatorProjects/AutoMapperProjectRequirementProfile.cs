using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.WebApp.Dtos;
using Portfolio.PorfolioDomain.Core.Entities;


namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperProjectRequirementProfile : Profile
    {
        public AutoMapperProjectRequirementProfile()
        {
            CreateMap<ProjectRequirementDto, ProjectRequirement>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.ProjectID))
                
                .ForMember(dest => dest.Requirement, opt => opt.MapFrom(src => src.Requirement))
          
                ;
            CreateMap<ProjectRequirement, ProjectRequirementDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.ProjectID))
                .ForMember(dest => dest.Requirement, opt => opt.MapFrom(src => src.Requirement))
                .ForMember(dest => dest.EditState, opt => opt.MapFrom(s => "ok"))
                ;
        }
    }
}
