using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperCertificationProfile : Profile
    {
        public AutoMapperCertificationProfile()
        {
            CreateMap<CertificationDto, Certification>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectCreatorID, opt => opt.MapFrom(src => src.ProjectCreatorID))
                // .ForMember(dest => dest.ProjectCreator, opt => opt.MapFrom(src => src.ProjectCreator))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CertID, opt => opt.MapFrom(src => src.CertID))
                .ForMember(dest => dest.IssuingBody_Name, opt => opt.MapFrom(src => src.IssuingBody_Name))
                .ForMember(dest => dest.IssuingBody_Logo, opt => opt.MapFrom(src => src.IssuingBody_Logo))
                .ForMember(dest => dest.CertName, opt => opt.MapFrom(src => src.CertName))
                .ReverseMap()
                ;
        }
    }
}
