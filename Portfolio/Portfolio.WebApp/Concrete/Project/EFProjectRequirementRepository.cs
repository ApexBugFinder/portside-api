using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Portfolio.WebApp.Concrete
{
    public class EFProjectRequirementRepository : IProjectRequirementRepository
    {

        private PortfolioContext Context;
        private string message;
        private string connString;

        public EFProjectRequirementRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
            this.connString = GetConnConstants.PortfolioDB;
        }
        public async Task<List<ProjectRequirement>> GetItemsByPC(string ProjectID)
        {
            List<ProjectRequirement> projectRequirements = new List<ProjectRequirement>();
            try
            {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectRequirementsByProjectID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pId", SqlDbType.VarChar).Value = ProjectID;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
            projectRequirements = await sqlReader.Getdata(reader);

          }
            await con.CloseAsync();
        }
                message = "Projects in DB: \n" + projectRequirements.ToList();



                if (projectRequirements == null)
                    throw new NullReferenceException("Did not return any Projects");

                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(ex.Message);
            }
            GC.Collect();
            return projectRequirements;
        }

        public async Task<ProjectRequirement> Create(ProjectRequirement ItemToCreate)
        {
            List<ProjectRequirement> results = new List<ProjectRequirement>();
            ProjectRequirement ItemCreated = new ProjectRequirement();
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.ID))
            {
                message = "Error: ProjectReqID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_createProjectRequirement ", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // SqlParameter tvp = cmd.Parameters.AddWithValue("@newCertType", certTble);
                        // tvp.SqlDbType = System.Data.SqlDbType.Structured;
                        // tvp.TypeName = "dbo.CertTblType";

                        cmd.Parameters.Add("@ProjectRequirementID", SqlDbType.VarChar).Value = ItemToCreate.ID;
                        cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToCreate.ProjectID;
                        cmd.Parameters.Add("@Requirement", SqlDbType.NText).Value = ItemToCreate.Requirement;



                        await con.OpenAsync();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
                        results = await sqlReader.Getdata(reader);

                    }
                        await con.CloseAsync();
                    }
                    if (results== null || results.Count == 0 ) {
                        message = "There was an error, the Project Requirement: " + ItemToCreate.ID + " was not returned from the Database";
                        throw new NullReferenceException(message);
                    }
                    else {
                        ItemCreated = results[0];

                        message = "Added ProjectRequirements to PortfolioDB: \n" + ItemCreated.ID + "\n";
                        Notification.PostMessage(message);
                    }
            }
            catch (Exception ex)
            {

                message = "Error writing ProjectRequirements to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            return ItemCreated;
        }
        public async Task<List<ProjectRequirement>> CreateRange(List<ProjectRequirement> ItemsToCreate)
        {
            // Checks

            try
            {
                Context.ProjectRequirements.AddRange(ItemsToCreate);
                await Context.SaveChangesAsync();
                message = "Added ProjectRequirements to PortfolioDB: \n" + ItemsToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing ProjectRequirements to PortfolioDB: \n" + ItemsToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return ItemsToCreate;
        }

        public async Task<List<ProjectRequirement>> DeleteItem(ProjectRequirement ItemToDelete)
        {
            List<ProjectRequirement> results = new List<ProjectRequirement>();


                try
                {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_deleteProjectRequirement", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToDelete.ID;
                            cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToDelete.ProjectID;
                            await con.OpenAsync();
                            SqlDataReader reader = cmd.ExecuteReader();
                            SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
                            results = await sqlReader.Getdata(reader);

                        }
                        await con.CloseAsync();
                    }


                }
                catch (Exception ex)
                {
                    message = "Deleting ProjectRequirements with ID " + ItemToDelete.ID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    GC.Collect();
                    throw new Exception(message);
                }

            GC.Collect();
            return results;
        }

        public async Task DeleteRange(List<ProjectRequirement> ItemToDelete)
        {


                try
                {

                    Context.ProjectRequirements.RemoveRange(ItemToDelete);
                await Context.SaveChangesAsync();

                message = "ProjectRequirements with ID: " + ItemToDelete.ToString() + " has been DELETED";
                    Notification.PostMessage(message);

                }
                catch (Exception ex)
                {
                    message = "Deleting ProjectRequirements with ID " + ItemToDelete.ToString() + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    GC.Collect();
                    throw new Exception(message);
                }


        }
        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.ProjectRequirements.AnyAsync(i => i.ID == ItemId);

            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for ProjectRequirements ID " + ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return Found;
        }

        public async Task<ProjectRequirement> Read(string ItemId)
        {
            List<ProjectRequirement> results = new List<ProjectRequirement>();
            ProjectRequirement Found = new ProjectRequirement();
            try
            {
        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectRequirementByID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemId;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
            results = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();

        }
        if (results.Count == 0 || results == null)
          throw new NullReferenceException("Project Requirement " + ItemId + " was not found");

                Found = results[0];
                message = "Found ProjectRequirements: " + Found.ID + " in the Portfolio DB";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for ProjectRequirements with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return Found;
        }

        public async Task<ProjectRequirement> Update(ProjectRequirement ItemToUpdate)
        {
            List<ProjectRequirement> results = new List<ProjectRequirement>();
            ProjectRequirement UpdatedItem = new ProjectRequirement();
            if (string.IsNullOrEmpty(ItemToUpdate.ID))
            {
                message = "Error: Project Requirement ID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.ID))
            {



                using (SqlConnection con = new SqlConnection(connString))
                {
                using (SqlCommand cmd = new SqlCommand("sp_updateProjectRequirement", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectID;
                    cmd.Parameters.Add("@Requirement", SqlDbType.NVarChar).Value = ItemToUpdate.Requirement;



                    await con.OpenAsync();
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
                    results = await sqlReader.Getdata(reader);

                }
                await con.CloseAsync();
                }
                if (results.Count == 0 || results == null)
                {
                message = "Updated Project Requirement " + ItemToUpdate.ID + " was not returned from database";
                Notification.PostMessage(message);
                GC.Collect();
                    throw new NullReferenceException(message);
                } else {
                    UpdatedItem = results[0];
                    message = "ProjectRequirements was updated in PortFolio DB to: \n" + JsonConvert.SerializeObject(UpdatedItem);
                Notification.PostMessage(message);
                }




            }
            else
            {
                message = "Update Database Error while updating ProjectRequirements with ID: " + ItemToUpdate.ID + "\n";
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return UpdatedItem;
        }

        public async Task<List<ProjectRequirement>> GetItems()
        {
            List<ProjectRequirement> itemsFound = new List<ProjectRequirement>();
            try
            {


                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_ProjectRequirements", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            await con.OpenAsync();
                            SqlDataReader reader = cmd.ExecuteReader();
                            SqlReaderProjectRequirement sqlReader = new SqlReaderProjectRequirement();
                            itemsFound = await sqlReader.Getdata(reader);

                        }
                        await con.CloseAsync();

                    }
                message = "ProjectRequirements in DB: \n" + itemsFound.ToList();



                if (itemsFound == null) {
                    GC.Collect();
                    throw new NullReferenceException("Did not return any Project Links");
                }


                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(ex.Message);
            }
            GC.Collect();
            return itemsFound;
        }

    Task IGeneric1KeyInterface<ProjectRequirement>.Delete(string ItemToDelete)
    {
      throw new NotImplementedException();
    }
  }
}
