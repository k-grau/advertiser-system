using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Advertisers.Models;


namespace Advertisers.DAL
{
    public class CompanyDataHandler
    {
      
        private string connectionString = "";

        public CompanyDataHandler(IConfiguration configuration) {
            connectionString = configuration.GetValue<string>("ConnectionStrings:CompanyDbConnectionString");
        }


        public int SetCompany(CompanyDetails Details, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC dbo.GetCompany @OrgNo = @orgNo, @Name = @name";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("orgNo", SqlDbType.Int).Value = Details.OrgNo;
            dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = Details.Name;

            SqlDataReader reader = null;
            errormsg = "";
           
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Details.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                }
                reader.Close();
                if(Details.CompanyId < 1) 
                {
                    errormsg = "Databasfel";
                    return 0;
                }
                return Details.CompanyId;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        } 


        public CompanyViewModel GetFullCompanyDetails (int companyId, int contactId, int billingId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC dbo.GetFullCompanyDetails @CompanyId = @companyId, @BillingId = @billingId, @ContactId = @contactId;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("companyId", SqlDbType.Int).Value = companyId;
            dbCommand.Parameters.Add("contactId", SqlDbType.Int).Value = contactId;
            dbCommand.Parameters.Add("billingId", SqlDbType.Int).Value = billingId;
           
            CompanyViewModel cvm = new CompanyViewModel();
            CompanyDetails cd = new CompanyDetails();
            CompanyContact cc = new CompanyContact();
            CompanyBilling cb = new CompanyBilling();
            SqlDataReader reader = null;
            errormsg = "";
           
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    cd.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                    cd.OrgNo = Convert.ToInt32(reader["OrgNo"]);
                    cd.Name = reader["Name"].ToString();

                    cc.ContactId = Convert.ToInt32(reader["ContactId"]);
                    cc.ContactAddress = reader["ContactAddress"].ToString();
                    cc.ContactPostcode = Convert.ToInt32(reader["ContactPostcode"]);
                    cc.ContactCity = reader["ContactCity"].ToString();
                    cc.ContactPhone = reader["ContactPhone"].ToString();

                    cb.BillingId = Convert.ToInt32(reader["BillingId"]);
                    cb.BillingAddress = reader["BillingAddress"].ToString();
                    cb.BillingPostcode = Convert.ToInt32(reader["BillingPostcode"]);
                    cb.BillingCity = reader["BillingCity"].ToString();
                       
                }
                cvm.Details = cd;
                cvm.Contact = cc;
                cvm.Billing = cb;

                reader.Close();
                   
                if(cvm.Details.CompanyId < 1) 
                {
                    errormsg = "Databasfel";
                    return null;
                }
                return cvm;
            }
            catch (Exception e)
            {

                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        } 



        public String GetCompanyName(int companyId, string errormsg)
        {
            
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT Name FROM Companies WHERE CompanyId = @companyId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("companyId", SqlDbType.Int).Value = Convert.ToInt32(companyId);
            
            string companyName = "";
            SqlDataReader reader = null;
            errormsg = "";
            
             try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    companyName = reader["Name"].ToString();                     
                }
                    reader.Close();
                    if(companyName == "") 
                    {
                        errormsg = "Databasfel";
                        return null;
                    }
                    return companyName;
                }
                catch (Exception e)
                {
                    errormsg = e.Message;
                    return null;
                }
                finally
                {
                    dbConnection.Close();
            }
        }



        public int SetCompanyContact(int companyId, CompanyContact contact, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC dbo.GetSetCompanyContact @CompanyId = @companyId, @Address = @contactAddress," 
            + " @City= @contactCity, @Postcode = @contactPostcode, @Phone = @contactPhone";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("companyId", SqlDbType.Int).Value = companyId;
            dbCommand.Parameters.Add("contactAddress", SqlDbType.VarChar, 50).Value = contact.ContactAddress;
            dbCommand.Parameters.Add("contactPostcode", SqlDbType.Int).Value = contact.ContactPostcode;
            dbCommand.Parameters.Add("contactCity", SqlDbType.VarChar, 50).Value = contact.ContactCity;
            dbCommand.Parameters.Add("contactPhone", SqlDbType.VarChar, 50).Value = contact.ContactPhone;

            SqlDataReader reader = null;
            errormsg = "";
           

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    contact.ContactId = Convert.ToInt32(reader["ContactId"]);
                }
                reader.Close();
                if(contact.ContactId < 1) 
                {
                    errormsg = "Databasfel";
                    return 0;
                }
                return contact.ContactId;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        } 



         public int SetCompanyBilling(int companyId, CompanyBilling billing, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC dbo.GetSetCompanyBilling @CompanyId = @companyId, @Address = @billingAddress," 
            + " @City= @billingCity, @Postcode = @billingPostcode";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("companyId", SqlDbType.Int).Value = companyId;
            dbCommand.Parameters.Add("billingAddress", SqlDbType.VarChar, 50).Value = billing.BillingAddress;
            dbCommand.Parameters.Add("billingPostcode", SqlDbType.Int).Value = billing.BillingPostcode;
            dbCommand.Parameters.Add("billingCity", SqlDbType.VarChar, 50).Value = billing.BillingCity;

            SqlDataReader reader = null;
            errormsg = "";
           

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    billing.BillingId = Convert.ToInt32(reader["billingId"]);
                }
                reader.Close();
                if(billing.BillingId < 1) 
                {
                    errormsg = "Databasfel.";
                    return 0;
                }
                return billing.BillingId;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        } 
    }
}