using Microsoft.EntityFrameworkCore;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Portfolio.WebApp.Concrete
{
  public class EFProjectRepository : IProjectRepository
  {
    private PortfolioContext Context;
    private string message;
    private string connString;

    public EFProjectRepository(PortfolioContext portfolioContext)
    {
      Context = portfolioContext;
      this.connString = GetConnConstants.PortfolioDB;

    }


        public async Task<Project> Create(Project ItemToCreate)
        {
          List<Project> results = new List<Project>();
          Project ItemCreated = new Project();
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.ProjectCreatorID))
            {
                message = "Error: ProjectCreatorID is null or empty";
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            try
            {


             using (SqlConnection con = new SqlConnection(connString))
              {
                using (SqlCommand cmd = new SqlCommand("sp_createProject", con))
                {
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToCreate.ID;
                  cmd.Parameters.Add("@ProjectName", SqlDbType.VarChar).Value = ItemToCreate.ProjectName;
                  cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToCreate.ProjectCreatorID;
                  cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ItemToCreate.Description;
                  cmd.Parameters.Add("@SDate", SqlDbType.DateTime2).Value = ItemToCreate.Started;
                  cmd.Parameters.Add("@CDate", SqlDbType.DateTime2).Value = ItemToCreate.Completed;
                  cmd.Parameters.Add("@Banner", SqlDbType.NText).Value = ItemToCreate.Banner;
                  cmd.Parameters.Add("@Published", System.Data.SqlDbType.Bit).Value = ItemToCreate.Published;

                  await con.OpenAsync();

                  SqlDataReader reader = cmd.ExecuteReader();

                  SqlReaderProject sqlReader = new SqlReaderProject();
                  results = await sqlReader.Getdata(reader);

                }
                await con.CloseAsync();
                GC.Collect();
              }
              if (results.Count == 0 || results == null)
              {
                message = "Nothing was returned from DB, there was a problem with the Create";
                Notification.PostMessage(message);
                throw new NullReferenceException(message);
              }
              else {
                ItemCreated = results[0];
                // message = "ADDED Project to PortfolioDB: \n" +ItemCreated.ID + "\n";
                // Notification.PostMessage(message);
              }
            }
            catch (Exception ex)
            {

                message = "Error writing Project to PortfolioDB: \n" +ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return ItemCreated;
        }


    public async Task Delete(string ItemToDeleteID)
    {
      List<Project> results = new List<Project>();
      if (await Exists(ItemToDeleteID))
      {

                try
                {

          using (SqlConnection con = new SqlConnection(connString))
          {
            using (SqlCommand cmd = new SqlCommand("sp_deleteProject", con))
            {
              cmd.CommandType = System.Data.CommandType.StoredProcedure;
              cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToDeleteID;


              await con.OpenAsync();

              SqlDataReader reader = cmd.ExecuteReader();

              SqlReaderProject sqlReader = new SqlReaderProject();
              results = await sqlReader.Getdata(reader);

            }
            await con.CloseAsync();
          }

          // message = "Project with ID: " + ItemToDeleteID + " has been DELETED";
          // Notification.PostMessage(message);

        }
        catch (Exception ex)
        {
          message = "Deleting Project with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
          Notification.PostMessage(message);
          GC.Collect();
          throw new Exception(message);
        }
        GC.Collect();
      }
      else
      {
        // message = "Project with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
        // Notification.PostMessage(message);
      }
    }

    public async Task<bool> Exists(string ItemId)
    {
      bool Found = false;
      try
      {
        Found = await Context.Projects.AnyAsync(i => i.ID == ItemId);

            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for Project ID " +
                    ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            GC.Collect();
            return Found;
        }

    public async Task<List<Project>> GetItems()
    {
      List<Project> results = new List<Project>();
      try{
              using (SqlConnection con = new SqlConnection(connString))
              {
                using (SqlCommand cmd = new SqlCommand("sp_Projects", con))
                {
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;

                  await con.OpenAsync();
                  SqlDataReader reader = cmd.ExecuteReader();
                  SqlReaderProject sqlReader = new SqlReaderProject();
                  results = await sqlReader.Getdata(reader);

                }

              await con.CloseAsync();
              }
              GC.Collect();
    }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        GC.Collect();
        throw new Exception(ex.Message);
  }
      return results;
    }
    public async Task<List<Project>> GetItemsByPCAsync(string ProjectCreatorID)
    {
      List<Project> projects = new List<Project>();
      try
      {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectsFullByProjectCreatorID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ProjectCreatorID;

            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProject sqlReader = new SqlReaderProject();
            projects = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();
          if (projects == null) {
              message = "Did not return any Projects";
              Notification.PostMessage(message);
              GC.Collect();
              throw new NullReferenceException(message);
          }
          else {
              // message = "Project found in the PortfolioDB: ";
              // foreach (var item in projects)
              // {
              //   message += message + "\n" + "ProjectID:  " + item.ID + "  :   Project Name: " + item.ProjectName;
              // }
              // Notification.PostMessage(message);
          }
        }

        GC.Collect();
      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        GC.Collect();
        throw new Exception(ex.Message);
      }
      return projects;

    }

        public async Task<Project> Read(string ItemId)
        {
            Project Found = new Project();
            List<Project> projects = new List<Project>();
            try
            {
        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_ProjectByProjectID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ItemId;

            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderProject sqlReader = new SqlReaderProject();
            projects = await sqlReader.Getdata(reader);
            await con.CloseAsync();
          }

          if (projects == null) {

          }
          if (projects.Count >0) {
            // message = "Project found in the PortfolioDB: ";
            Found = projects[0];


          // message += message + "\n" + "id:  " + Found.ID + "  :   ProjectName: " + Found.ProjectName;

          // Notification.PostMessage(message);


          }
  GC.Collect();

        }
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for Project with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                GC.Collect();
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<Project> Update(Project ItemToUpdate)
        {

          List<Project> results = new List<Project>();
          Project updatedProject = new Project();
            if (string.IsNullOrEmpty(ItemToUpdate.ID))
            {
                message = "Error: ProjectID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

    if (await Exists(ItemToUpdate.ID))
    {
      try {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_updateProject", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@updateID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
                    cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectCreatorID;
                    cmd.Parameters.Add("@ProjectName", SqlDbType.NVarChar).Value = ItemToUpdate.ProjectName;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ItemToUpdate.Description;
                    cmd.Parameters.Add("@SDate", SqlDbType.DateTime2).Value = ItemToUpdate.Started;
                    cmd.Parameters.Add("@CDate", SqlDbType.DateTime2).Value = ItemToUpdate.Completed;
                    cmd.Parameters.Add("@Banner", SqlDbType.NText).Value = ItemToUpdate.Banner;
                    cmd.Parameters.Add("@Published", SqlDbType.Bit).Value = ItemToUpdate.Published;



                    await con.OpenAsync();
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlReaderProject sqlReader = new SqlReaderProject();
                    results = await sqlReader.Getdata(reader);

                }
                await con.CloseAsync();
                if (results.Count > 0) {
                  updatedProject = results[0];
                  message = "Project Creator was updated in PortFolio DB to: \n" + updatedProject;
                  Notification.PostMessage(message);
                }
                else {
                    message = "Project:  " + ItemToUpdate.ID + ".  No data was returned from the database";
                    Notification.PostMessage(message);
                    throw new NullReferenceException(message);
                }
              }


        GC.Collect();

      }
      catch (Exception ex) {
          message = "Reading Database Error:\nMessage:  " + ex.Message;
          Notification.PostMessage(message);
          throw new Exception(ex.Message);
      }


      }
      else
      {
        message = "Update Database Error while updating Project with ID: " + ItemToUpdate.ID + "\n Project was not found";
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      return updatedProject;
    }

    Task<List<Project>> IGeneric1KeyInterface<Project>.GetItemsByPC(string ItemId)
    {
      throw new NotImplementedException();
    }
  }
}
