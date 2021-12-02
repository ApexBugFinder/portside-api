using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperProjectLinkProfile : Profile
    {
        public AutoMapperProjectLinkProfile()
        {
            CreateMap<ProjectLinkDto, ProjectLink>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.ProjectID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))

                .ReverseMap()
                ;
        }
    }
}
