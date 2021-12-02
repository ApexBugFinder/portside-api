using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Abstract
{
    public interface IProjectCreatorRepository : IGeneric1KeyInterface<ProjectCreator>
    {
        Task<ProjectCreator> GetItemByPC (string itemID);
        Task<ProjectCreator> GetItemByUserName(string username);
       // Task<List<ProjectCreator>> GetItemsByPC(string itemID);

        Task<List<ProjectCreator>> SearchForPCsByKeyword(string keyword);
//    List<ProjectCreator> SearchForPCsByKeyword(string keyword);
  }
}
