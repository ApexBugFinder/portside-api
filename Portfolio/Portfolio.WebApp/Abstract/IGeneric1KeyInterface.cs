using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Abstract
{
    public interface IGeneric1KeyInterface<T>
    {
        Task<T> Create(T ItemToCreate);

        Task<T> Read(string ItemId);
        Task<T> Update(T ItemToUpdate);

        Task Delete(string ItemToDelete);

        Task<bool> Exists(string ItemId);
        Task<List<T>> GetItemsByPC(string ItemId);



        Task<List<T>> GetItems();
    
  }
}
