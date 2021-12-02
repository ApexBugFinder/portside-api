using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Helpers
{
    public class DtoEntityConverter<TDto, TEntity>
    {
        private readonly IMapper _mapper;
        private string message = "";
        private TDto ConvertedToDto;
        private TEntity ConvertedToEntity;

        public DtoEntityConverter()
        {
            _mapper = new Helpers.MapperConfiguration().Configure();
        }
        public TEntity ConvertToCoreEntity(TDto dto)
        {
            try
            {
                ConvertedToEntity = (TEntity) _mapper.Map<TEntity>(dto);
            }
            catch (AutoMapperMappingException ex)
            {
                message = "***ERROR AUTOMAPPING***" +
                        "\nIN CLASS: " + dto.GetType().Name +
                        "\nWITH METHOD: " + MethodBase.GetCurrentMethod().Name +
                        "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                throw new AutoMapperMappingException(message);
            }

            return ConvertedToEntity;
        }

        public TDto ConvertToDto(TEntity EntityToConvert)
        {

            try
            {
                ConvertedToDto = _mapper.Map<TDto>(EntityToConvert);
            }

            catch (AutoMapperMappingException ex)
            {
                message = "***ERROR AUTOMAPPING***" +
                        "\nIN CLASS: " + EntityToConvert.GetType().Name +
                        "\nWITH METHOD: " + MethodBase.GetCurrentMethod().Name +
                        "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                throw new AutoMapperMappingException(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ConvertedToDto;
        }

    }
}
