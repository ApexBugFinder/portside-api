using Microsoft.EntityFrameworkCore;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Portfolio.WebApp.Concrete
{
  public class EFDegreeRepository : IDegreeRepository
  {
    private PortfolioContext Context;
    private string message;
    private IConfiguration Configuration { get; set; }
    private string connString { get; set; }

    public EFDegreeRepository(PortfolioContext portfolioContext)
    {
      message = "";
      Context = portfolioContext;
      this.connString = GetConnConstants.PortfolioDB;
    }


    public async Task<Degree> Create(Degree ItemToCreate)
    {
      List<Degree> results = new List<Degree>();
      Degree certCreated = new Degree();
      message = "ItemToCreate data: \n" +
      "ID: " + ItemToCreate.ID +
      "\nMinors: " + ItemToCreate.Minors +
      "\nDegree Name: " + ItemToCreate.DegreeName;
      Notification.PostMessage(message);
      // Checks
      if (string.IsNullOrEmpty(ItemToCreate.ProjectCreatorID))
      {
        message = "Error: ProjectCreatorID is null or empty";
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      try
      {
        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_createDegree", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@DegreeID", SqlDbType.VarChar).Value = ItemToCreate.ID;
            cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToCreate.ProjectCreatorID;
            cmd.Parameters.Add("@DegreeType", SqlDbType.NVarChar).Value = ItemToCreate.DegreeType;
            cmd.Parameters.Add("@DegreeName", SqlDbType.NVarChar).Value = ItemToCreate.DegreeName;
            cmd.Parameters.Add("@Minors", SqlDbType.NText).Value = ItemToCreate.Minors;
            cmd.Parameters.Add("@Institution", SqlDbType.NVarChar).Value = ItemToCreate.Institution;
            cmd.Parameters.Add("@InstitutionLogo", SqlDbType.NText).Value = ItemToCreate.InstitutionLogo;
            cmd.Parameters.Add("@DegCity", SqlDbType.NVarChar).Value = ItemToCreate.City;
            cmd.Parameters.Add("@DegState", SqlDbType.NVarChar).Value = ItemToCreate.State;

            SqlParameter graduatedParam = new SqlParameter();
            graduatedParam.DbType = System.Data.DbType.Boolean;
            graduatedParam.ParameterName = "@Graduated";
            graduatedParam.Value = ItemToCreate.Graduated;
            cmd.Parameters.Add(graduatedParam);
            cmd.Parameters.Add("@GraduationYear", SqlDbType.DateTime2).Value = ItemToCreate.GraduationYear;



            // cmd.Parameters.Add("@Minors", SqlDbType.NVarChar).Value = ItemToCreate.Minors;
            // cmd.Parameters.Add("@Institution", SqlDbType.NVarChar).Value = ItemToCreate.Institution;
            // cmd.Parameters.Add("@InstitutionLogo", SqlDbType.NText).Value = ItemToCreate.InstitutionLogo;
            // cmd.Parameters.Add("@DegCity", SqlDbType.NVarChar).Value = ItemToCreate.City;
            // cmd.Parameters.Add("@DegState", SqlDbType.NVarChar).Value = ItemToCreate.State;



            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderDegree sqlReader = new SqlReaderDegree();
            results = await sqlReader.Getdata(reader);

          }
        }
      }
      catch (InvalidOperationException ex)
      {
        message = "Error Getting User from DB using Username: " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }
      if (results.Count > 0)
      {
        return results[0];
      }
      else
      {
        message = "Updating Database Error";
        Notification.PostMessage(message);
        throw new Exception(message);
      }

    }


    public async Task Delete(string ItemToDeleteID)
    {
      if (await Exists(ItemToDeleteID))
      {

        try
        {
          Degree itemToDelete = await Read(ItemToDeleteID);
          Context.Degrees.Remove(itemToDelete);
          await Context.SaveChangesAsync();

          message = "Degree with ID: " + ItemToDeleteID + " has been DELETED";
          Notification.PostMessage(message);

        }
        catch (Exception ex)
        {
          message = "Deleting Degree with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
          Notification.PostMessage(message);
          throw new Exception(message);
        }

      }
      else
      {
        message = "Degree with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
        Notification.PostMessage(message);
      }
    }

    public async Task<bool> Exists(string ItemId)
    {
      bool Found = false;
      try
      {
        Found = await Context.Degrees.AnyAsync(i => i.ID == ItemId);

      }
      catch (Exception ex)
      {
        message = "Reading Database Error looking for Degree ID " + ItemId + "\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      return Found;
    }

    public async Task<List<Degree>> GetItems()
    {
      List<Degree> itemsFound = new List<Degree>();
      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_Degrees", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderDegree sqlReader = new SqlReaderDegree();
            itemsFound = await sqlReader.Getdata(reader);

          }

        }

        return itemsFound;

      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }

    }
    public async Task<List<Degree>> GetItemsByPC(string ID)
    {
      List<Degree> items = new List<Degree>();
      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_DegreesByProjectCreatorID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ID;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderDegree sqlReader = new SqlReaderDegree();
            items = await sqlReader.Getdata(reader);

          }

        }

        return items;

      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }

    }

    public async Task<Degree> Read(string ItemId)
    {

      List<Degree> items = new List<Degree>();
      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_CertByID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ItemId;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderDegree sqlReader = new SqlReaderDegree();
            items = await sqlReader.Getdata(reader);

          }

        }




      }
      catch (NullReferenceException ex)
      {
        message = "Certification not found in the DB: " + ItemId + ".\n" + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }

      if (items.Count > 0)
      {

        return items[0];
      }
      else
      {
        message = "Certification not found in the DB: " + ItemId;
        Notification.PostMessage(message);
        throw new Exception(message);
      }

    }

    public async Task<Degree> Update(Degree ItemToUpdate)
    {
  
      List<Degree> results = new List<Degree>();
      if (string.IsNullOrEmpty(ItemToUpdate.ID))
      {
        message = "Error: DegreeID is null or empty";
        Notification.PostMessage(message);

        throw new Exception(message);
      }

      if (await Exists(ItemToUpdate.ID))
      {

        try
        {
          using (SqlConnection con = new SqlConnection(connString))
          {
            using (SqlCommand cmd = new SqlCommand("sp_updateDegree", con))
            {
              cmd.CommandType = System.Data.CommandType.StoredProcedure;
              cmd.Parameters.Add("@DegreeID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
              cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectCreatorID;
              cmd.Parameters.Add("@DegreeType", SqlDbType.NVarChar).Value = ItemToUpdate.DegreeType;
              cmd.Parameters.Add("@DegreeName", SqlDbType.NVarChar).Value = ItemToUpdate.DegreeName;
              cmd.Parameters.Add("@Minors", SqlDbType.NText).Value = ItemToUpdate.Minors;
              cmd.Parameters.Add("@Institution", SqlDbType.NVarChar).Value = ItemToUpdate.Institution;
              cmd.Parameters.Add("@InstitutionLogo", SqlDbType.NText).Value = ItemToUpdate.InstitutionLogo;
              cmd.Parameters.Add("@DegCity", SqlDbType.NVarChar).Value = ItemToUpdate.City;
              cmd.Parameters.Add("@DegState", SqlDbType.NVarChar).Value = ItemToUpdate.State;
              SqlParameter graduatedParam = new SqlParameter();
              graduatedParam.DbType = System.Data.DbType.Boolean;
              graduatedParam.ParameterName = "@Graduated";
              graduatedParam.Value = ItemToUpdate.Graduated;
              cmd.Parameters.Add(graduatedParam);
              cmd.Parameters.Add("@GraduationYear", SqlDbType.DateTime2).Value = ItemToUpdate.GraduationYear;


              con.Open();
              SqlDataReader reader = cmd.ExecuteReader();
              SqlReaderDegree sqlReader = new SqlReaderDegree();
              results = await sqlReader.Getdata(reader);

            }
          }
        }
        catch (InvalidOperationException ex)
        {
          message = "Error Getting User from DB using Username: " + ex.Message;
          Notification.PostMessage(message);
          throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
          message = "Reading Database Error:\nMessage:  " + ex.Message;
          Notification.PostMessage(message);
          throw new Exception(ex.Message);
        }

      }
      else
      {
        message = "Update Database Error while updating Degree with ID: " + ItemToUpdate.ID + "\n";
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      return ItemToUpdate;
    }



  }
}
