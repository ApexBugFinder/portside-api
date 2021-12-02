using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.WebApp.Dtos;
using Portfolio.WebApp.Helpers.AutoMapperProfiles;



namespace Portfolio.WebApp.Helpers
{
    public class MapperConfiguration
    {
        // private readonly MapperProfileOptions Options;

        public IMapper Configure()
        {
           
           
           var config = new AutoMapper.MapperConfiguration(cfg =>
            {
               
                cfg.AddProfile<AutoMapperProjectCreatorProfile>();
                cfg.AddProfile<AutoMapperProjectProfile>();
                cfg.AddProfile<AutoMapperProjectLinkProfile>();
                cfg.AddProfile<AutoMapperProjectRequirementProfile>();
                cfg.AddProfile<AutoMapperPublishedHistoryProfile>();
                cfg.AddProfile<AutoMapperExperienceProfile>();
                cfg.AddProfile<AutoMapperRoleProfile>();
                cfg.AddProfile<AutoMapperCertificationProfile>();
                cfg.AddProfile<AutoMapperDegreeProfile>();

                
               
               
            });
            // config.CreateMapper();
            return config.CreateMapper();
        }

    
    
        
    }
   
}
