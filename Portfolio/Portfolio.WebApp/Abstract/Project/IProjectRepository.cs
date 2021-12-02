using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Abstract
{
    public interface IProjectRepository: IGeneric1KeyInterface<Project>
    {
    Task<List<Project>> GetItemsByPCAsync(string ID);

    }
}
