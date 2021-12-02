using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Concrete
{
  public class EFGenericRepository<T>
  {
        private PortfolioContext Context;
        private string message;
        private Type typeofParameter = typeof(T);

        public EFGenericRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
        }

        public async Task<T> Create<T>(T ItemToCreate) where T : class
        {
            // Checks

            try
            {
                Context.Set<T>().Add(ItemToCreate);
                await Context.SaveChangesAsync();
                message = "Added Item to PortfolioDB: \n" + ItemToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing " + ItemToCreate.GetType() + " Project to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToCreate;
        }

        //public async Task Delete<T>(string ItemToDeleteID)
        //{
        //    if (await Exists(ItemToDeleteID))
        //    {

        //        try
        //        {
        //            T itemToDelete = await Read(ItemToDeleteID);
        //            Context.Set<T>().Remove(itemToDelete);
        //            await Context.SaveChangesAsync();

        //            message = "Item " + typeofParameter.Name + "  with ID: " + ItemToDeleteID + " has been DELETED";
        //            Notification.PostMessage(message);

        //        }
        //        catch (Exception ex)
        //        {
        //            message = "Deleting " + typeofParameter.Name + " with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
        //            Notification.PostMessage(message);
        //            throw new Exception(message);
        //        }

        //    }
        //    else
        //    {
        //        message = typeofParameter.Name + " with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
        //        Notification.PostMessage(message);
        //    }
        //}

        //public async Task<bool> Exists<T>(string ItemId) where T: class
        //{
        //    bool Found = false;
        //    try
        //    {

        //        Found = Context.Set<T>().Any(ItemId);

        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Reading Database Error looking for ProjectCreator ID " + ItemId + "\nMessage:  " + ex.Message;
        //        Notification.PostMessage(message);
        //        throw new Exception(message);
        //    }
        //    return Found;
        //}


    }
}
