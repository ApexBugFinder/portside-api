using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Portfolio.WebApp.Concrete
{
    public class EFProjectLinkRepository : IProjectLinkRepository
    {

        private PortfolioContext Context;
        private string message;
        private string connString;
        public EFProjectLinkRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
            this.connString = GetConnConstants.PortfolioDB;
        }
        public async Task<List<ProjectLink>> CreateItem(ProjectLink ItemToCreate)
        {
            List<ProjectLink> results = new List<ProjectLink>();
            ProjectLink ItemCreated = new ProjectLink();
            if (string.IsNullOrEmpty(ItemToCreate.Service))
            {
                message = "Error Project link must have a service type when posting Project Link: " + ItemToCreate;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            if (string.IsNullOrEmpty(ItemToCreate.ProjectID))
            {
                message = "Error when Posting to Project Link to PortfolioDB.  Project link must have a Project ID: " + ItemToCreate;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            if (await Exists(ItemToCreate.ID))
            {

            }
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_createProjectLink ", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // SqlParameter tvp = cmd.Parameters.AddWithValue("@newCertType", certTble);
                        // tvp.SqlDbType = System.Data.SqlDbType.Structured;
                        // tvp.TypeName = "dbo.CertTblType";

                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToCreate.ID;
                        cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToCreate.ProjectID;
                        cmd.Parameters.Add("@Link", SqlDbType.NText).Value = ItemToCreate.Link;
                        cmd.Parameters.Add("@Service", SqlDbType.NText).Value = ItemToCreate.Service;
                        cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = ItemToCreate.Title;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ItemToCreate.Description;


                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
                        results = await sqlReader.Getdata(reader);

                    }
                    }

                message = "Added Project Link to PortfolioDB: \n" + ItemToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing Link to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return results;
        }
        public async Task<List<ProjectLink>> CreateRange(List<ProjectLink> ItemsToCreate)
        {
            // Checks

            try
            {

                Context.Links.AddRange(ItemsToCreate);
                await Context.SaveChangesAsync();
                message = "Added ProjectLinks to PortfolioDB: \n" + ItemsToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing ProjectLinks to PortfolioDB: \n" + ItemsToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemsToCreate;
        }

        public async Task<List<ProjectLink>> UpdateRange(List<ProjectLink> ItemsToCreate)
        {
            // Checks

            try
            {

                Context.Links.UpdateRange(ItemsToCreate);
                await Context.SaveChangesAsync();
                message = "Updated ProjectLinks to PortfolioDB: \n" + ItemsToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing ProjectLinks to PortfolioDB: \n" + ItemsToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemsToCreate;
        }
        public async Task<List<ProjectLink>> DeleteItem(ProjectLink ItemToDelete)
        {
            List<ProjectLink> results = new List<ProjectLink>();

               try
                {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_deleteProjectLink", con))
                        {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToDelete.ID;
                        cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToDelete.ProjectID;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
                        results = await sqlReader.Getdata(reader);

                        }
                    }
                    message = "ProjectLink with ID: " + ItemToDelete.ID + " has been DELETED";
                    Notification.PostMessage(message);

                }
                catch (Exception ex)
                {
                    message = "Deleting ProjectLink with ID " + ItemToDelete.ID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    throw new Exception(message);
                }
            return results;

        }

        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.Links.AnyAsync(i => i.ID == ItemId);

            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for ProjectLink ID " + ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<List<ProjectLink>> GetItemsByPC(string projectId)
        {
            List<ProjectLink> items = new List<ProjectLink>();
            try
            {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_LinksByProjectID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pId", SqlDbType.VarChar).Value = projectId;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
            items = await sqlReader.Getdata(reader);

          }

        }


                message = "ProjectLinks in DB: \n" + items.ToList();



                if (items == null)
                    throw new NullReferenceException("Did not return any ProjectLinks");

                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            return items;
        }

        public async Task<List<ProjectLink>> GetItems()
        {
            List<ProjectLink> itemsFound = new List<ProjectLink>();
            try
            {


                using (SqlConnection con = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProjectLinks", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
                        itemsFound = await sqlReader.Getdata(reader);

                    }

                }


                message = "Project Links in DB: \n" ;
                foreach(var item in itemsFound) {
                    message += message + "ID:  " + item.ID + "  ProjectLink: " + item.Link;
                }

                Notification.PostMessage(message);

                if (itemsFound == null || itemsFound.Count ==0)
                    throw new NullReferenceException("Did not return any Project Links");


            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            return itemsFound;
        }

        public async Task<ProjectLink> Read(string ItemId)
        {
            List<ProjectLink> results = new List<ProjectLink>();
            ProjectLink Found = new ProjectLink();
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_ProjectLinkByID", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ItemId;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
                        results = await sqlReader.Getdata(reader);

                    }

                    }
                if (results.Count==0 || results == null)
                    throw new NullReferenceException("Project Link " + ItemId + " was not found");
                Found = results[0];
                message = "Found Project Link: " + Found.ID + " in the Portfolio DB";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for ProjectLink with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<ProjectLink> Update(ProjectLink ItemToUpdate)
        {

            List<ProjectLink> results = new List<ProjectLink>();
            ProjectLink UpdatedProjectLink = new ProjectLink();
            if (string.IsNullOrEmpty(ItemToUpdate.Service))
            {
                message = "Error: Project Link Service is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.ID))
            {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_updateProjectLink", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //   SqlParameter tvp = cmd.Parameters.AddWithValue("@toUpdateType", ItemToUpdate);
            //   tvp.SqlDbType = System.Data.SqlDbType.Structured;
            //   tvp.TypeName = "dbo.CertTblType";
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
            cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectID;
            cmd.Parameters.Add("@Link", SqlDbType.NText).Value = ItemToUpdate.Link;
            cmd.Parameters.Add("@Service", SqlDbType.NText).Value = ItemToUpdate.Service;
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = ItemToUpdate.Title;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ItemToUpdate.Description;


            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectLink sqlReader = new SqlReaderProjectLink();
            results = await sqlReader.Getdata(reader);

          }
        }
        if (results.Count == 0 || results == null) {
            message = "Updated Project Link " + ItemToUpdate.ID + " was not returned from database";
            Notification.PostMessage(message);
          throw new NullReferenceException(message);
        }
                UpdatedProjectLink = results[0];

                message = "Project Link was updated in PortFolio DB to: \n" + ItemToUpdate.ID;
                Notification.PostMessage(message);

            }
            else
            {
                message = "Read Database Error while looking for ProjectLink with ID: " + ItemToUpdate.ID + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return UpdatedProjectLink;
        }

    Task IGeneric1KeyInterface<ProjectLink>.Delete(string ItemToDelete)
    {
      throw new NotImplementedException();
    }

    Task<ProjectLink> IGeneric1KeyInterface<ProjectLink>.Create(ProjectLink ItemToCreate)
    {
      throw new NotImplementedException();
    }
  }
}
