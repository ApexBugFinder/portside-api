using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Helpers
{
    public class DataConverter<TDto>
    {
        public TDto GetOkObjectResult(ActionResult<TDto> entity)
        {
            var result = (OkObjectResult)entity.Result;

            if (result == null)
            {
                var other = entity.Value;
                return other;
            }
            TDto returnResult = (TDto)result.Value;
            return returnResult;
        }

    }
}
