using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApp.Abstract;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Portfolio.WebApp.Concrete
{
    public class EFExperienceRepository : IExperienceRepository
    {
        private PortfolioContext Context;
        private string message;
        private string connString {get; set; }

        public EFExperienceRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
            this.connString = GetConnConstants.PortfolioDB;
        }


        public async Task<Experience> Create(Experience ItemToCreate)
        {
            List<Experience> results = new List<Experience>();
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.ProjectCreatorID))
            {
                message = "Error: ExperienceCreatorID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }


            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                using (SqlCommand cmd = new SqlCommand("sp_createExperience", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    // SqlParameter tvp = cmd.Parameters.AddWithValue("@newCertType", certTble);
                    // tvp.SqlDbType = System.Data.SqlDbType.Structured;
                    // tvp.TypeName = "dbo.CertTblType";

                    cmd.Parameters.Add("@ExperienceID", SqlDbType.VarChar).Value = ItemToCreate.ID;
                    cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToCreate.ProjectCreatorID;
                    cmd.Parameters.Add("@Company", SqlDbType.NVarChar).Value = ItemToCreate.Company;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = ItemToCreate.Title;
                    cmd.Parameters.Add("@LogoUrl", SqlDbType.NText).Value = ItemToCreate.LogoUrl;
                    cmd.Parameters.Add("@SDate", SqlDbType.DateTime2).Value = ItemToCreate.Started;
                    cmd.Parameters.Add("@CDate", SqlDbType.DateTime2).Value = ItemToCreate.Completed;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = ItemToCreate.City;
                    cmd.Parameters.Add("@MyState", SqlDbType.NVarChar).Value = ItemToCreate.State;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlReaderExperience sqlReader = new SqlReaderExperience();
                    results = await sqlReader.Getdata(reader);

                }
                }
            }
            catch (Exception ex)
            {

                message = "Error writing Experience to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToCreate;
        }


        public async Task Delete(Experience ItemToDelete)
        {

                try
                {

                    Context.Experiences.Remove(ItemToDelete);
                    await Context.SaveChangesAsync();

                    message = "Experience with ID: " + ItemToDelete.ID + " has been DELETED";
                    Notification.PostMessage(message);

                }
                catch (Exception ex)
                {
                    message = "Deleting Experience with ID " + ItemToDelete.ID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    throw new Exception(message);
                }
        }

        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.Experiences.AnyAsync(i => i.ID == ItemId);
            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for Experience ID " + ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<List<Experience>> GetItems()
        {
            List<Experience> itemsFound = new List<Experience>();
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_Experiences", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderExperience sqlReader = new SqlReaderExperience();
                        itemsFound = await sqlReader.Getdata(reader);

                    }

                    }





            if (itemsFound.Count() == 0) {
                message = "Did not return any Experiences";
                Notification.PostMessage(message);
                throw new NullReferenceException(message);
            }



            }
            catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            return itemsFound;
        }
        public async Task<List<Experience>> GetItemsByPC(string ID)
        {
            List<Experience> items = new List<Experience>();
            try
            {
using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ExperiencesByProjectCreatorID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ID;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderExperience sqlReader = new SqlReaderExperience();
            items = await sqlReader.Getdata(reader);

          }

        }


            message = "Experiences returned from DB: \n" ;
            foreach(var item in items) {
                message += "\nID:  " + item.ID + "  Company: " + item.Company;

            }

            Notification.PostMessage(message);
        if (items.Count == 0) {
            message = "Did not return any Experiences";
            Notification.PostMessage(message);
          throw new NullReferenceException(message);
        }
        else {
                return items;
        }


   }
    catch (Exception ex)
            {
                message = "Reading Database Error:\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Experience> Read(string ItemId)
        {
            List<Experience> results = new List<Experience>();
            Experience Found = new Experience();
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_ExperiencesByID", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ItemId;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderExperience sqlReader = new SqlReaderExperience();
                        results = await sqlReader.Getdata(reader);
                        Found = results[0];
                    }

                    }
                message = "Found Experience: " + ItemId + " in the Portfolio DB";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for Experience Creator with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<Experience> Update(Experience ItemToUpdate)
        {
            List<Experience> results = new List<Experience>();
            Experience ItemUpdated = new Experience();
            if (string.IsNullOrEmpty(ItemToUpdate.ID))
            {
                message = "Error: ExperienceID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.ID))
            {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_updateExperience", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //   SqlParameter tvp = cmd.Parameters.AddWithValue("@toUpdateType", ItemToUpdate);
            //   tvp.SqlDbType = System.Data.SqlDbType.Structured;
            //   tvp.TypeName = "dbo.CertTblType";
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
            cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectCreatorID;
            cmd.Parameters.Add("@Company", SqlDbType.NVarChar).Value = ItemToUpdate.Company;
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = ItemToUpdate.Title;
            cmd.Parameters.Add("@LogoUrl", SqlDbType.NText).Value = ItemToUpdate.LogoUrl;
            cmd.Parameters.Add("@SDate", SqlDbType.DateTime2).Value = ItemToUpdate.Started;
            cmd.Parameters.Add("@CDate", SqlDbType.DateTime2).Value = ItemToUpdate.Completed;
            cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = ItemToUpdate.City;
            cmd.Parameters.Add("@MyState", SqlDbType.NVarChar).Value = ItemToUpdate.State;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderExperience sqlReader = new SqlReaderExperience();
            results = await sqlReader.Getdata(reader);
      ItemUpdated = results[0];
          }
        }
            }
            else
            {
                message = "Update Database Error while updating Experience with ID: " + ItemToUpdate.ID + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            if (results.Count == 0 || results ==null) {
                message = "Nothing was returned from the database";
                Notification.PostMessage(message);
                throw new NullReferenceException(message);
            }
            else {
                ItemUpdated = results[0];
            }
            return ItemUpdated;
        }

    Task IGeneric1KeyInterface<Experience>.Delete(string ItemToDelete)
    {
      throw new NotImplementedException();
    }
  }
}
