using Microsoft.EntityFrameworkCore;
using Portfolio.PorfolioDomain.Core.Entities;

using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Concrete
{
    public class EFPublishedHistoryRepository : IPublishedHistoryRepository
    {
        private PortfolioContext Context;
        private string message;

        public EFPublishedHistoryRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
        }
        public async Task<List<PublishedHistory>> GetItemsByPC(string ProjectID)
        {
            List<PublishedHistory> publishedHistories = new List<PublishedHistory>();
            try
            {


                publishedHistories = await Context.ProjectPublishHistory.Where(i => i.ProjectID == ProjectID).ToListAsync();
                message = "Projects in DB: \n" + publishedHistories.ToList();



                if (publishedHistories == null)
                    throw new NullReferenceException("Did not return any Projects");

                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            return publishedHistories;
        }

        public async Task<PublishedHistory> Create(PublishedHistory ItemToCreate)
        {
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.ID))
            {
                message = "Error: PublishedHistoryID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            try
            {
                Context.ProjectPublishHistory.Add(ItemToCreate);
                await Context.SaveChangesAsync();
                message = "Added PublishedHistory to PortfolioDB: \n" + ItemToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing PublishedHistory to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToCreate;
        }


        public async Task Delete(string ItemToDeleteID)
        {
            if (await Exists(ItemToDeleteID))
            {

                try
                {
                    PublishedHistory itemToDelete = await Read(ItemToDeleteID);
                    Context.ProjectPublishHistory.Remove(itemToDelete);
                    await Context.SaveChangesAsync();

                    message = "PublishedHistory with ID: " + ItemToDeleteID + " has been DELETED";
                    Notification.PostMessage(message);

                }
                catch (Exception ex)
                {
                    message = "Deleting PublishedHistory with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    throw new Exception(message);
                }

            }
            else
            {
                message = "PublishedHistory with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
                Notification.PostMessage(message);
            }
        }

        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.ProjectPublishHistory.AnyAsync(i => i.ID == ItemId);

            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for PublishedHistory ID " + ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<PublishedHistory> Read(string ItemId)
        {
            PublishedHistory Found = new PublishedHistory();
            try
            {
                Found = await Context.ProjectPublishHistory.FindAsync(ItemId);
                message = "Found PublishedHistory: " + ItemId + " in the Portfolio DB";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for PublishedHistory with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<PublishedHistory> Update(PublishedHistory ItemToUpdate)
        {
            if (string.IsNullOrEmpty(ItemToUpdate.ID))
            {
                message = "Error: PublishedHistory ID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.ID))
            {


                // USERNAME UPDATED
                Context.ProjectPublishHistory.Update(ItemToUpdate);
                await Context.SaveChangesAsync();
                message = "PublishedHistory was updated in PortFolio DB to: \n" + ItemToUpdate;
                Notification.PostMessage(message);

            }
            else
            {
                message = "Update Database Error while updating PublishedHistory with ID: " + ItemToUpdate.ID + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToUpdate;
        }

        public async Task<List<PublishedHistory>> GetItems()
        {
            List<PublishedHistory> itemsFound = new List<PublishedHistory>();
            try
            {


                itemsFound = await Context.ProjectPublishHistory.ToListAsync();
                message = "PublishedHistorys in DB: \n" + itemsFound.ToList();



                if (itemsFound == null)
                    throw new NullReferenceException("Did not return any Project Links");

                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            return itemsFound;
        }
    }
}
