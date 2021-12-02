using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperProjectProfile : Profile
    {
        public AutoMapperProjectProfile()
        {
            CreateMap<ProjectDto, Project>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.ProjectCreatorID, opt => opt.MapFrom(src => src.ProjectCreatorID))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Started, opt => opt.MapFrom(src => src.Started))
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.Completed))
                .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Banner))
                .ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.Published))
              //  .ForMember(dest => dest.ProjectRequirements, opt => opt.MapFrom(src => src.ProjectRequirements))
                .ReverseMap()
                ;
        }
    }
}
