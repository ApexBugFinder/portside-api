using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Abstract
{
    public interface IProjectLinkRepository : IGeneric1KeyInterface<ProjectLink>
    {
        Task<List<ProjectLink>> CreateRange(List<ProjectLink> projectLinks);

        Task<List<ProjectLink>> DeleteItem(ProjectLink ItemToDelete);

        Task<List<ProjectLink>> CreateItem(ProjectLink ItemToCreate);
        Task<List<ProjectLink>> UpdateRange(List<ProjectLink> projectLinks);
    }
}
