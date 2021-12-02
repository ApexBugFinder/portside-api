using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Abstract
{
    public interface IExperienceRepository : IGeneric1KeyInterface<Experience>
    {
       Task Delete(Experience ItemToDelete);
    }
}
