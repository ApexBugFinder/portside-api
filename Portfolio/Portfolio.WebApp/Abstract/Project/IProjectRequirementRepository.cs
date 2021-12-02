using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Abstract
{
    public interface IProjectRequirementRepository: IGeneric1KeyInterface<ProjectRequirement>
    {
        Task<List<ProjectRequirement>> CreateRange(List<ProjectRequirement> projectRequirements);
        Task DeleteRange(List<ProjectRequirement> projectRequirements);

        Task<List<ProjectRequirement>> DeleteItem(ProjectRequirement ItemToDeleteID);
    }
}
