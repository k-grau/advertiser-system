using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Subscribers.Models;

namespace Subscribers.DAL
{
    public class DataHandler
    {
        private string connectionString = ""; 

        public DataHandler(IConfiguration configuration) {
            connectionString = configuration.GetValue<string>("ConnectionStrings:DbConnectionString");
        }
        
        

        public SubscriberDetails GetSubscriber(int subscriberNo, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC dbo.GetSubscriber @SubscriberNo = @subscriberNo;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("subscriberNo", SqlDbType.Int).Value = subscriberNo;

            SqlDataReader reader = null;
            errormsg = "";
            SubscriberDetails sd = new SubscriberDetails();
            ContactDetails cd = new ContactDetails();


            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    sd.SubscriberNo = Convert.ToInt32(reader["SubscriberNo"]);
                    sd.PersonalNo = Convert.ToInt64(reader["PersonalNo"]);
                    sd.Firstname = reader["Firstname"].ToString();
                    sd.Lastname = reader["Lastname"].ToString();

                    cd.Address = reader["Address"].ToString();
                    cd.Postcode = Convert.ToInt32(reader["Postcode"]);
                    cd.City = reader["City"].ToString();
                    cd.Phone = reader["Phone"].ToString();

                    sd.Contact = cd;
                }
                reader.Close();
                if(sd.SubscriberNo < 1) 
                {
                    errormsg = "No such post found";
                    return null;
                }
                return sd;
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








        public List<SubscriberDetails> GetSubscribers(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "EXEC GetSubscribers;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            

            SqlDataReader reader = null;
            errormsg = "";
            List<SubscriberDetails> sdl = new List <SubscriberDetails>();
            
            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    SubscriberDetails sd = new SubscriberDetails();
                    sd.SubscriberNo = Convert.ToInt32(reader["SubscriberNo"]);
                    sd.PersonalNo = Convert.ToInt64(reader["PersonalNo"]);
                    sd.Firstname = reader["Firstname"].ToString();
                    sd.Lastname = reader["Lastname"].ToString();

                    ContactDetails cd = new ContactDetails();
                    cd.Address = reader["Address"].ToString();
                    cd.Postcode = Convert.ToInt32(reader["Postcode"]);
                    cd.City = reader["City"].ToString();
                    cd.Phone = reader["Phone"].ToString();

                    sd.Contact = cd;
                    sdl.Add(sd);
                }
                reader.Close();
                
                return sdl;
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


        public int UpdateSubscriber(SubscriberDetails sd, int subscriberNo, out string errormsg)
        {
            if(sd.SubscriberNo != subscriberNo) {
                errormsg = "Entry did not match route. Database was not updated.";
                return 0;
            }

            PropertyInfo[] subscriberProps = typeof(SubscriberDetails).GetProperties();
            PropertyInfo[] contactProps = typeof(ContactDetails).GetProperties();
            String sqlstring = "EXEC dbo.UpdateSubscribers ";
           
            foreach(PropertyInfo subprop in subscriberProps) {
                object subscriberValue = subprop.GetValue(sd, null); 

                if(!subprop.ToString().Contains("Contact")) 
                {
                    sqlstring += BuildSqlParameter(subprop, subscriberValue);
                    
                } else if(subprop.ToString().Contains("Contact")) 
                {
                    foreach(PropertyInfo conprop in contactProps) {
                            object contactValue = conprop.GetValue(sd.Contact, null);
                            sqlstring += BuildSqlParameter(conprop, contactValue); 
                    }

                }
            }
            sqlstring = sqlstring.Remove(sqlstring.Length - 2) + ";";
           

            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = connectionString;
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);


            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i != 0) { errormsg = ""; }
                else { errormsg = "Failed: could not save to database";}
                return (i);
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

        public String BuildSqlParameter(PropertyInfo prop, object value) {
            string[] parameter;
            String sqlParameter = "";
           
            if(value is int || value is long) 
            {
                if(value is int) 
                {
                    if(Convert.ToInt32(value) == 0) 
                    {
                        value = null;
                    }
                } else if(value is long) 
                {
                    if(Convert.ToInt64(value) == 0) 
                    {
                        value = null;
                    }
                }           
            }

            if(value != null)
            {
                parameter = prop.ToString().Split(" ");
                sqlParameter = "@" + parameter[1] + "='" + value + "', ";
                return sqlParameter;
                
            }
            return sqlParameter;
        }

    }
}