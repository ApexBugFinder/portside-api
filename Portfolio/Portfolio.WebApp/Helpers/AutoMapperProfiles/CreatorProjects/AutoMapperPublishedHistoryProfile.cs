using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;

namespace Portfolio.WebApp.Helpers.AutoMapperProfiles
{
    public class AutoMapperPublishedHistoryProfile: Profile
    {
        public AutoMapperPublishedHistoryProfile()
        {
            CreateMap<PublishedHistoryDto, PublishedHistory>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.ProjectID))

                .ForMember(dest => dest.PublishedOn, opt => opt.MapFrom(src => src.PublishedOn))
                .ForMember(dest => dest.UnPublishedOn, opt => opt.MapFrom(src => src.UnPublishedOn))
                .ReverseMap()
                ;


        }
    }
}
