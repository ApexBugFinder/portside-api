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
  public class EFCertificationRepository : ICertificationRepository
  {
    private PortfolioContext Context;
    private string message;
    private IConfiguration Configuration { get; set; }
    private string connString { get; set; }

    public EFCertificationRepository(PortfolioContext portfolioContext)
    {
      this.message = "";
      Context = portfolioContext;
      this.connString = GetConnConstants.PortfolioDB;
    }


    public async Task<Certification> Create(Certification ItemToCreate)
    {
      List<Certification> results = new List<Certification>();
      Certification certCreated = new Certification();
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
          using (SqlCommand cmd = new SqlCommand("sp_createCert", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@CertID", SqlDbType.VarChar).Value = ItemToCreate.CertID;
            cmd.Parameters.Add("@IssuingBody_Name", SqlDbType.NVarChar).Value = ItemToCreate.IssuingBody_Name;
            cmd.Parameters.Add("@IssuingBody_Logo", SqlDbType.NText).Value = ItemToCreate.IssuingBody_Logo;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToCreate.ID;
            cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToCreate.ProjectCreatorID;
            cmd.Parameters.Add("@CertName", SqlDbType.NVarChar).Value = ItemToCreate.CertName;

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@IsActive";
            param1.Value = ItemToCreate.IsActive;
            param1.DbType = System.Data.DbType.Boolean;
            cmd.Parameters.Add(param1);

            await  con.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            SqlReaderCertification sqlReader = new SqlReaderCertification();
            results = await sqlReader.Getdata(reader);
            certCreated = results[0];
          }
          await con.CloseAsync();
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
      GC.Collect();
      return certCreated;
    }


    public async Task Delete(string ItemToDeleteID)
    {
      if (await Exists(ItemToDeleteID))
      {

        try
        {
          Certification itemToDelete = await Read(ItemToDeleteID);
          Context.Certifications.Remove(itemToDelete);
          await Context.SaveChangesAsync();

          message = "Certification with ID: " + ItemToDeleteID + " has been DELETED";
          Notification.PostMessage(message);
         
          GC.Collect();
        }
        catch (Exception ex)
        {
          message = "Deleting Certification with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
          Notification.PostMessage(message);
          throw new Exception(message);
        }
        GC.Collect();
      }
      else
      {
        message = "Certification with ID: " + ItemToDeleteID + " does not Exist, WARNING: cannot DELETE";
        Notification.PostMessage(message);
      }
    }

    public async Task<bool> Exists(string ItemId)
    {
      bool Found = false;
      try
      {
        Found = await Context.Certifications.AnyAsync(i => i.ID == ItemId);

      }
      catch (Exception ex)
      {
        message = "Reading Database Error looking for Certification ID " + ItemId + "\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(message);
      }
      GC.Collect();
      return Found;
    }

    public async Task<List<Certification>> GetItems()
    {
      List<Certification> itemsFound = new List<Certification>();


      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_Certs", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderCertification sqlReader = new SqlReaderCertification();
            itemsFound = await sqlReader.Getdata(reader);
            await con.CloseAsync();
          }

        }
        GC.Collect();
        return itemsFound;

      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }

    }
    public async Task<List<Certification>> GetItemsByPC(string ID)
    {
      List<Certification> items = new List<Certification>();
      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_CertsByProjectCreatorID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@pcId", SqlDbType.VarChar).Value = ID;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderCertification sqlReader = new SqlReaderCertification();
            items = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();
        }
        GC.Collect();
        return items;

      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }

    }

    public async Task<Certification> Read(string ItemId)
    {
      Certification Found = new Certification();
      List<Certification> items = new List<Certification>();
      try
      {

        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_CertByID", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ItemId;
            await con.OpenAsync();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderCertification sqlReader = new SqlReaderCertification();
            items = await sqlReader.Getdata(reader);

          }
          await con.CloseAsync();
        }




      }
      catch (NullReferenceException ex)
      {
        message = "Certification not found in the DB: " + ItemId + ".\n" + ex.Message;
        Notification.PostMessage(message);
        GC.Collect();
        throw new Exception(message);
      }
      catch (Exception ex)
      {
        message = "Reading Database Error:\nMessage:  " + ex.Message;
        Notification.PostMessage(message);
        throw new Exception(ex.Message);
      }
      GC.Collect();
      if (items.Count > 0)
      {

        return items[0];
      }
      else
      {
        message = "Certification not found in the DB: " + ItemId;
        Notification.PostMessage(message);
        GC.Collect();
        throw new Exception(message);
      }



    }

    public async Task<Certification> Update(Certification ItemToUpdate)
    {
      List<Certification> results = new List<Certification>();
      if (string.IsNullOrEmpty(ItemToUpdate.ID))
      {
        message = "Error: CertificationID is null or empty";
        Notification.PostMessage(message);
        throw new Exception(message);
      }


        try
        {
          using (SqlConnection con = new SqlConnection(connString))
          {
            using (SqlCommand cmd = new SqlCommand("sp_updateCert", con))
            {
              cmd.CommandType = System.Data.CommandType.StoredProcedure;
              //   SqlParameter tvp = cmd.Parameters.AddWithValue("@toUpdateType", ItemToUpdate);
              //   tvp.SqlDbType = System.Data.SqlDbType.Structured;
              //   tvp.TypeName = "dbo.CertTblType";
              cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
              cmd.Parameters.Add("@ProjectCreatorID", SqlDbType.VarChar).Value = ItemToUpdate.ProjectCreatorID;
              cmd.Parameters.Add("@CertName", SqlDbType.NVarChar).Value = ItemToUpdate.CertName;
              SqlParameter param1 = new SqlParameter();
              param1.ParameterName = "@IsActive";
              param1.Value = ItemToUpdate.IsActive;
              param1.DbType = System.Data.DbType.Boolean;
              cmd.Parameters.Add(param1);
              cmd.Parameters.Add("@CertId", SqlDbType.VarChar).Value = ItemToUpdate.CertID;
              cmd.Parameters.Add("@IssuingBody_Name", SqlDbType.NVarChar).Value = ItemToUpdate.IssuingBody_Name;
              cmd.Parameters.Add("@IssuingBody_Logo", SqlDbType.NText).Value = ItemToUpdate.IssuingBody_Logo;

              await con.OpenAsync();
              SqlDataReader reader = cmd.ExecuteReader();
              SqlReaderCertification sqlReader = new SqlReaderCertification();
              results = await sqlReader.Getdata(reader);

            }
          await con.CloseAsync();
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

      GC.Collect();
      return ItemToUpdate;
    }


  }
}
