using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperDegreeProfile : Profile
    {
        public AutoMapperDegreeProfile()
        {
            CreateMap<DegreeDto, Degree>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.DegreeType, opt => opt.MapFrom(src => src.DegreeType))
                .ForMember(dest => dest.ProjectCreatorID, opt => opt.MapFrom(src => src.ProjectCreatorID))
                // .ForMember(dest => dest.ProjectCreator, opt => opt.MapFrom(src => src.ProjectCreator))
                .ForMember(dest => dest.DegreeName, opt => opt.MapFrom(src => src.DegreeName))
                .ForMember(dest => dest.Minors, opt => opt.MapFrom(src => src.Minors))
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.Institution))
                .ForMember(dest => dest.InstitutionLogo, opt => opt.MapFrom(src => src.InstitutionLogo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Graduated, opt => opt.MapFrom(src => src.Graduated))
                .ForMember(dest => dest.GraduationYear, opt => opt.MapFrom(src => src.GraduationYear))
                .ReverseMap()
                ;
        }
    }
}
