using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Services;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.WebApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApp;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Web.Helpers;
using Newtonsoft.Json;


namespace Portfolio.WebApp.Concrete
{
  public class EFProjectCreatorRepository : IProjectCreatorRepository
  {
        private PortfolioContext Context;
        private IConfiguration Configuration {get; set;}
        private string message;
        private  string connString;


    public EFProjectCreatorRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
            this.connString = GetConnConstants.PortfolioDB;
        }

    public EFProjectCreatorRepository()
    {
      this.connString = GetConnConstants.PortfolioDB;
    }

    public async Task<List<ProjectCreator>> GetItems()
        {
            List<ProjectCreator> users = new List<ProjectCreator>();
      try
      {
        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectCreators", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            await con.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
            users = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();

          // message = "Project Creators found in the PortfolioDB: ";
          // foreach(var item in users) {
          //     message += message + "\n" + "id:  " +item.SubjectId + "  :   Username: " + item.Username;
          // }
          // Notification.PostMessage(message);
        }
        GC.Collect();
      }
      catch (Exception ex)
      {
        // message = "Reading Database Error:\nMessage:  " + ex.Message;
        // Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }
            return users;
        }
        public async Task<ProjectCreator> Create(ProjectCreator ItemToCreate)
        {

          List<ProjectCreator> results = new List<ProjectCreator>();
          ProjectCreator CreatedUser = new ProjectCreator();
          // message = "Item to Create Id: " + ItemToCreate.SubjectId;
          // Notification.PostMessage(message);
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.Username))
            {
                message = "Error: UserName is null or empty";
                // Notification.PostMessage(message);
                throw new Exception(message);
            }
            try
            {
              using (SqlConnection con = new SqlConnection(connString))
              {
                using (SqlCommand cmd = new SqlCommand("sp_createProjectCreator", con))
                {
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = ItemToCreate.SubjectId;
                  cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = ItemToCreate.Username;
                  // SqlParameter newUser = cmd.Parameters.AddWithValue("@id", JsonConvert.SerializeObject(ItemToCreate.SubjectId));
                  // SqlParameter newUserID = cmd.Parameters.AddWithValue("@username", JsonConvert.SerializeObject(ItemToCreate.Username));
                  await con.OpenAsync();

                  SqlDataReader reader = cmd.ExecuteReader();

                  SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
                  results = await sqlReader.Getdata(reader);

                }
          await con.CloseAsync();
          GC.Collect();
          // message = "Project Creator was updated in PortFolio DB to: \n" + "id:   " +  ItemToCreate.SubjectId + "     Username:  " + ItemToCreate.Username;
          // Notification.PostMessage(message);
        }

        GC.Collect();
      }
            catch (Exception ex)
            {
              // message = "Reading Database Error:\nMessage:  " + ex.Message;
              // Notification.PostMessage(message);
              throw new Exception(ex.Message);
            }
            return ItemToCreate;
        }
        public async Task Delete(string ItemToDeleteID)
        {
            if (await Exists(ItemToDeleteID))
            {
                try
                {
                    ProjectCreator itemToDelete = await Read(ItemToDeleteID);
                    Context.Users.Remove(itemToDelete);
                    await Context.SaveChangesAsync();

          // message = "Project Creator with ID: " + ItemToDeleteID + " has been DELETED";
          // Notification.PostMessage(message);
        }
                catch (Exception ex)
                {
                    // message = "Deleting Project Creator with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message +"\n";
                    // Notification.PostMessage(message);
                    throw new Exception(ex.Message);
                }
        GC.Collect();
      } else
            {
                // message = "ProjectCreator with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
                // Notification.PostMessage(message);
            }
        }

        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.Users.AnyAsync(i => i.SubjectId == ItemId);

            } catch (Exception ex)
            {
                // message = "Reading Database Error looking for ProjectCreator ID " + ItemId + "\nMessage:  " + ex.Message;
                // Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
      GC.Collect();
      return Found;
        }

        public async Task<ProjectCreator> Read(string ItemId)
        {
            ProjectCreator Found = new ProjectCreator();
            List<ProjectCreator> results = new List<ProjectCreator>();
            try
            {
              using (SqlConnection con = new SqlConnection(connString))
              {
                using (SqlCommand cmd = new SqlCommand("sp_ProjectCreatorByProjectCreatorID", con))
                {
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ItemId;
                  await con.OpenAsync();
                  SqlDataReader reader = cmd.ExecuteReader();
                  SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
                  results = await sqlReader.Getdata(reader);

                }
          await con.CloseAsync();
        }
        GC.Collect();
      }
            catch (Exception ex)
            {
              // message = "Reading Database Error:\nMessage:  " + ex.Message;
              // Notification.PostMessage(message);
              throw new Exception(ex.Message);
            }
            if (results.Count > 0)
            {
              Found = results[0];
              // message = "Project Creator was updated in PortFolio DB to: \n" + "id:   " + Found.SubjectId + "     Username:  " + Found.Username;
              // Notification.PostMessage(message);
            } else {
                // message = "Project Creator was not Found";
                // Notification.PostMessage(message);
                throw new Exception(message);
            }


            return Found;
        }

        public async Task<ProjectCreator> Update(ProjectCreator ItemToUpdate)
        {
      ProjectCreator user = new ProjectCreator();
      List<ProjectCreator> results = new List<ProjectCreator>();

            if (string.IsNullOrEmpty(ItemToUpdate.Username))
            {
                // message = "Error: UserName is null or empty";
                // Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.SubjectId))
            {

        try
        {
          using (SqlConnection con = new SqlConnection(connString))
          {
            using (SqlCommand cmd = new SqlCommand("sp_updateProjectCreator", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToUpdate.SubjectId;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = ItemToUpdate.Username;
                cmd.Parameters.Add("@userPicUrl", SqlDbType.NText).Value = ItemToUpdate.userPicUrl;



                await con.OpenAsync();
                SqlDataReader reader = cmd.ExecuteReader();
                SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
                results = await sqlReader.Getdata(reader);

            }
            await con.CloseAsync();
            // message = "Project Creator was updated in PortFolio DB to: \n" + ItemToUpdate;
            // Notification.PostMessage(message);
          }
          GC.Collect();
        }
        catch (Exception ex)
        {
          // message = "Reading Database Error:\nMessage:  " + ex.Message;
          // Notification.PostMessage(message);
          throw new Exception(ex.Message);
        }
                        }
            else
            {
                // message = "Read Database Error while looking for Project Creator with ID: " + ItemToUpdate.SubjectId + "\n";
                // Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToUpdate;
        }

// Get Project Creators By ProjectCreatorID
        public async Task<ProjectCreator> GetItemByPC(string ItemId)
        {

      try
      {
        ProjectCreator user = new ProjectCreator();
        List<ProjectCreator> results = new List<ProjectCreator>();

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectCreatorByProjectCreatorID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ItemId;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
            results = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();
          GC.Collect();
        }
        user = results[0];
        GC.Collect();
        return user;

      }
      catch (Exception ex)
      {
        // message = "Reading Database Error:\nMessage:  " + ex.Message;
        // Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }
        }
    public async Task<ProjectCreator> GetItemByUserName(string userName)
    {
    if (string.IsNullOrEmpty(userName))
    {
        throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));
    }
      List<ProjectCreator> results = new List<ProjectCreator>();
        ProjectCreator user = new ProjectCreator();

    try {
      using (SqlConnection con = new SqlConnection(connString))
      {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectCreatorByUsername", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pcUname", SqlDbType.VarChar).Value = userName;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
            results = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();

        }
        GC.Collect();
    }
            catch (InvalidOperationException ex)
            {
                // message = "Error Getting User from DB using Username: " + ex.Message;
                // Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                // message = "Reading Database Error:\nMessage:  " + ex.Message;
                // Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }




        if (results.Count > 0){

        user = results[0];

        // message = "ProjectCreator in DB: \n" + user.SubjectId;
        // Notification.PostMessage(message);
        }

        if (user == null)
        throw new NullReferenceException("Did not return any ProjectCreators");

        GC.Collect();
        return user;
    }

        public async Task<List<ProjectCreator>> SearchForPCsByKeyword(string keyword)
        {
          var id = "";
            List<ProjectCreator> results = new List<ProjectCreator>();
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException($"'{nameof(keyword)}' cannot be null or empty.", nameof(keyword));
            }
            try {
                using (SqlConnection con = new SqlConnection(connString))
                {
                        using (SqlCommand cmd = new SqlCommand("sp_SearchProjectCreatorsByUsername", con))
                        {
                          cmd.CommandType = System.Data.CommandType.StoredProcedure;
                          cmd.Parameters.Add("@pcUname", SqlDbType.NVarChar).Value = keyword;
                          await con.OpenAsync();
                          SqlDataReader reader = cmd.ExecuteReader();
                          SqlReaderProjectCreator sqlReader = new SqlReaderProjectCreator();
                          results = await sqlReader.Getdata(reader);

                        }
          await con.CloseAsync();
        }
        GC.Collect();
      }
            catch (InvalidCastException ex) {


        //       results.ForEach(i => {
        //         id = i.SubjectId;
        //       });
        // message = "Error Getting User from DB using Username: " + ex.Message  + " id:" + id;
        // Notification.PostMessage(message);
        throw new Exception(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                message = "Error Getting User from DB using Username: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                // message = "Reading Database Error:\nMessage:  " + ex.Message;
                // Notification.PostMessage(message);
                throw new Exception(ex.Message);
            }
      GC.Collect();
      return results.ToList();
        }
    public Task<List<ProjectCreator>> GetItemsByPC(string ItemId) => throw new NotImplementedException();

    public bool Equals(EFProjectCreatorRepository other)
    {
      throw new NotImplementedException();
    }


  }
}
