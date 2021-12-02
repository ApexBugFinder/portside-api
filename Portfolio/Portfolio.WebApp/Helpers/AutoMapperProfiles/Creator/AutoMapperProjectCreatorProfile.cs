using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperProjectCreatorProfile : Profile
    {
        public AutoMapperProjectCreatorProfile()
        {
            CreateMap<ProjectCreatorDto, ProjectCreator>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.userPicUrl, opt => opt.MapFrom(src => src.UserPicUrl))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
                .ForMember(dest => dest.Certifications, opt => opt.MapFrom(src => src.Certifications))
                .ForMember(dest => dest.Degrees, opt => opt.MapFrom(src => src.Degrees))
                .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Experiences))
                .ReverseMap();
        }
    }
}
