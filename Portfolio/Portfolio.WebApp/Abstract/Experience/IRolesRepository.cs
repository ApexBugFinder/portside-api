using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Abstract
{
    public interface IRolesRepository : IGeneric1KeyInterface<Role>
    {
        Task<List<Role>> CreateRange(List<Role> Roles);

        Task<List<Role>> UpdateRange(List<Role> Roles);
        Task DeleteRange(List<Role> Roles);
        new Task<List<Role>> Delete(String ItemToDeleteID);
        new Task<List<Role>> Create(Role ItemToCreate);
    }
}
