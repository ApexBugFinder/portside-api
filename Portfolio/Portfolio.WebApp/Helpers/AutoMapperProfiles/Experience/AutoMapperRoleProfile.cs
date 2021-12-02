
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperRoleProfile : Profile
    {
        public AutoMapperRoleProfile()
        {
            CreateMap<RoleDto, Role>()
             .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ExperienceID, opt => opt.MapFrom(src => src.ExperienceID))

                .ForMember(dest => dest.MyRole, opt => opt.MapFrom(src => src.MyRole))
                .ForMember(dest => dest.MyTitle, opt => opt.MapFrom(src => src.MyTitle))

                ;

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ExperienceID, opt => opt.MapFrom(src => src.ExperienceID))
                .ForMember(dest => dest.MyRole, opt => opt.MapFrom(src => src.MyRole))
                .ForMember(dest => dest.MyTitle, opt => opt.MapFrom(src => src.MyTitle))

                .ForMember(dest => dest.EditState, opt => opt.MapFrom(s => "ok"))
                ;
        }
    }
}

