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

namespace Portfolio.WebApp.Concrete
{
    public class EFRoleRepository : IRolesRepository
    {
        private PortfolioContext Context;
        private string message;
        private string connString;

        public EFRoleRepository(PortfolioContext portfolioContext)
        {
            Context = portfolioContext;
            this.connString = GetConnConstants.PortfolioDB;
        }


        public async Task<List<Role>> Create(Role ItemToCreate)
        {
            List<Role> results = new List<Role>();
            // Checks
            if (string.IsNullOrEmpty(ItemToCreate.ExperienceID))
            {
                message = "Error: RoleCreatorID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            try
            {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_createRole ", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // SqlParameter tvp = cmd.Parameters.AddWithValue("@newCertType", certTble);
                        // tvp.SqlDbType = System.Data.SqlDbType.Structured;
                        // tvp.TypeName = "dbo.CertTblType";

                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToCreate.ID;
                        cmd.Parameters.Add("@ExperienceID", SqlDbType.VarChar).Value = ItemToCreate.ExperienceID;
                        cmd.Parameters.Add("@MyTitle", SqlDbType.NVarChar).Value = ItemToCreate.MyTitle;
                        cmd.Parameters.Add("@MyRole", SqlDbType.NVarChar).Value = ItemToCreate.MyRole;


                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderRole sqlReader = new SqlReaderRole();
                        results = await sqlReader.Getdata(reader);

                    }
                    }
            }
            catch (Exception ex)
            {

                message = "Error writing Role to PortfolioDB: \n" + ItemToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return results;
        }


        public async Task<List<Role>> CreateRange(List<Role> ItemsToCreate)
        {

            try
            {
                Context.Roles.AddRange(ItemsToCreate);
                await Context.SaveChangesAsync();
                message = "Added ProjectRequirements to PortfolioDB: \n" + ItemsToCreate + "\n";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {

                message = "Error writing Roles to PortfolioDB: \n" + ItemsToCreate + "\n" + "Error Message: " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemsToCreate;
        }



        public async Task<List<Role>> Delete(string ItemToDeleteID)
        {
            List<Role> results = new List<Role>();


                try
                {
                    Role ItemToDelete = await Read(ItemToDeleteID);
                     using (SqlConnection con = new SqlConnection(connString))
                    {
                    using (SqlCommand cmd = new SqlCommand("sp_deleteRole ", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // SqlParameter tvp = cmd.Parameters.AddWithValue("@newCertType", certTble);
                        // tvp.SqlDbType = System.Data.SqlDbType.Structured;
                        // tvp.TypeName = "dbo.CertTblType";

                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToDelete.ID;
                        cmd.Parameters.Add("@ExperienceID", SqlDbType.VarChar).Value = ItemToDelete.ExperienceID;



                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        SqlReaderRole sqlReader = new SqlReaderRole();
                        results = await sqlReader.Getdata(reader);

                    }
                    }
                    return results;
                }
                catch (Exception ex)
                {
                    message = "Deleting Role with ID " + ItemToDeleteID + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                    Notification.PostMessage(message);
                    throw new Exception(message);
                }


        }

        public async Task DeleteRange(List<Role> ItemToDelete)
        {


            try
            {

                Context.Roles.RemoveRange(ItemToDelete);
                await Context.SaveChangesAsync();
                message = "";
                ItemToDelete.ForEach(i =>
                {
                    message += message + "\n Role with ID: " + i.ID + " has been deleted";
                });
                Notification.PostMessage(message);

            }
            catch (Exception ex)
            {
                message = "Deleting Roles" + ItemToDelete + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }


        }

        public async Task<bool> Exists(string ItemId)
        {
            bool Found = false;
            try
            {
                Found = await Context.Roles.AnyAsync(i => i.ID == ItemId);

            }
            catch (Exception ex)
            {
                message = "Reading Database Error looking for Role ID " + ItemId + "\nMessage:  " + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return Found;
        }

        public async Task<List<Role>> GetItems()
        {
            List<Role> itemsFound = new List<Role>();
            try
            {


        using (SqlConnection con = new SqlConnection(connString))
        {
          using (SqlCommand cmd = new SqlCommand("sp_Roles", con))
          {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            SqlReaderRole sqlReader = new SqlReaderRole();
            itemsFound = await sqlReader.Getdata(reader);

          }

        }

                message = "Roles in DB: \n" + itemsFound.ToList();



                if (itemsFound == null)
                    throw new NullReferenceException("Did not return any Roles");

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
        public async Task<List<Role>> GetItemsByPC(string ID)
        {
            List<Role> items = new List<Role>();
            try
            {


                using (SqlConnection con = new SqlConnection(connString))
                {
                using (SqlCommand cmd = new SqlCommand("sp_RolesByExperienceID", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ExperienceId", SqlDbType.VarChar).Value = ID;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlReaderRole sqlReader = new SqlReaderRole();
                    items = await sqlReader.Getdata(reader);

                }

                }




                if (items == null)
                    throw new NullReferenceException("Did not return any Roles");
                message = "Roles in DB: \n" + items.ToList();
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

        public async Task<Role> Read(string ItemId)
        {
            Role Found = new Role();
            List<Role> results = new List<Role>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                        using (SqlCommand cmd = new SqlCommand("sp_RolesByID", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemId;
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            SqlReaderRole sqlReader = new SqlReaderRole();
                            results = await sqlReader.Getdata(reader);

                        }

                }
                if (results.Count == 0) {
                    message = "User was not found in the database";
                    Notification.PostMessage(message);
                    throw new NullReferenceException(message);
                }
                Found = results[0];
                message = "Found Role: " + Found.ID+ " in the Portfolio DB";
                Notification.PostMessage(message);
            }
            catch (Exception ex)
            {
                message = "Read Database Error while looking for Role Creator with ID: " + ItemId + "\n" + ex.Message;
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            return Found;
        }

        public async Task<Role> Update(Role ItemToUpdate)
        {

            Role Updated = new Role();
            List<Role> results = new List<Role>();

            if (string.IsNullOrEmpty(ItemToUpdate.ID))
            {
                message = "Error: RoleID is null or empty";
                Notification.PostMessage(message);
                throw new Exception(message);
            }

            if (await Exists(ItemToUpdate.ID))
            {


        // USERNAME UPDATED
                using (SqlConnection con = new SqlConnection(connString))
                {
                using (SqlCommand cmd = new SqlCommand("sp_updateCert", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //   SqlParameter tvp = cmd.Parameters.AddWithValue("@toUpdateType", ItemToUpdate);
            //   tvp.SqlDbType = System.Data.SqlDbType.Structured;
            //   tvp.TypeName = "dbo.CertTblType";
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = ItemToUpdate.ID;
                    cmd.Parameters.Add("@ExperienceID", SqlDbType.VarChar).Value = ItemToUpdate.ExperienceID;
                    cmd.Parameters.Add("@MyTitle", SqlDbType.NVarChar).Value = ItemToUpdate.MyTitle;
                    cmd.Parameters.Add("@MyRole", SqlDbType.NVarChar).Value = ItemToUpdate.MyRole;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlReaderRole sqlReader = new SqlReaderRole();
                    results = await sqlReader.Getdata(reader);

                }
                }
                message = "Role was updated in PortFolio DB to: \n" + ItemToUpdate;
                Notification.PostMessage(message);

            }
            else
            {
                message = "Update Database Error while updating Role with ID: " + ItemToUpdate.ID + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
            return ItemToUpdate;
        }

        public async Task<List<Role>> UpdateRange(List<Role> ItemsToUpdate)
        {

            try
            {

                Context.Roles.UpdateRange(ItemsToUpdate);
                await Context.SaveChangesAsync();
                message = "";
                ItemsToUpdate.ForEach(i =>
                {
                    message += message + "\n Role with ID: " + i.ID + " has been updated";
                });
                Notification.PostMessage(message);
                return ItemsToUpdate;
            }
            catch (Exception ex)
            {
                message = "Updating Roles" + ItemsToUpdate + " from Database has failed.   \nError: Message:  " + ex.Message + "\n";
                Notification.PostMessage(message);
                throw new Exception(message);
            }
        }

    Task<Role> IGeneric1KeyInterface<Role>.Create(Role ItemToCreate)
    {
      throw new NotImplementedException();
    }

    Task IGeneric1KeyInterface<Role>.Delete(string ItemToDelete)
    {
      throw new NotImplementedException();
    }
  }
}
