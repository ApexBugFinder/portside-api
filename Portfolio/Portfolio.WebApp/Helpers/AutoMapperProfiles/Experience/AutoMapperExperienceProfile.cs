using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperExperienceProfile : Profile
    {
        public AutoMapperExperienceProfile()
        {
            CreateMap<ExperienceDto, Experience>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectCreatorID, opt => opt.MapFrom(src => src.ProjectCreatorID))

                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))

                .ForMember(dest => dest.Started, opt => opt.MapFrom(src => src.Started))
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.Completed))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
                .ReverseMap();

        }
    }
}

