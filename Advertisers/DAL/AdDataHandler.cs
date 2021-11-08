using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Advertisers.Models;


namespace Advertisers.DAL
{
    public class AdDataHandler
    {

        private string connectionString = "";

        public AdDataHandler(IConfiguration configuration) {
            connectionString = configuration.GetValue<string>("ConnectionStrings:AdvertisementDbConnectionString");
        }

        public int CreateAdd(AdDetails ad, out string errormsg)
        {
            string billingIdParameter = "";
            if (ad.Ad_AdvertiserBillingId != 0)
            {
                billingIdParameter = ", @AdvertiserBillingId = @advertiserBillingId";
    
            }
            
            
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "EXEC dbo.CreateAdd @Headline = @headline, @Content = @content, @ArticlePrice = @articlePrice, "
            + "@AdPriceId = @priceId, @AdTypeId = @typeId, @AdvertiserId = @advertiserId, @AdvertiserAddressId = @advertiserAddressId" 
            + billingIdParameter + ";";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("headline", SqlDbType.VarChar, 30).Value = ad.Ad_Headline;
            dbCommand.Parameters.Add("content", SqlDbType.VarChar, 200).Value = ad.Ad_Content;
            dbCommand.Parameters.Add("articlePrice", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_ArticlePrice);
            dbCommand.Parameters.Add("advertiserId", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_AdvertiserId);
            dbCommand.Parameters.Add("priceId", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_AdPriceId);
            dbCommand.Parameters.Add("typeId", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_TypeId);
            dbCommand.Parameters.Add("advertiserAddressId", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_AdvertiserAddressId);
            
            if (ad.Ad_AdvertiserBillingId != 0)
            {
                dbCommand.Parameters.Add("advertiserBillingId", SqlDbType.Int).Value = Convert.ToInt32(ad.Ad_AdvertiserBillingId);
            }

            SqlDataReader reader = null;
            errormsg = "";
            
             try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    ad.Ad_Id = Convert.ToInt32(reader["Ad_Id"]);
                }
                    reader.Close();
                    if(ad.Ad_Id < 1) 
                    {
                        errormsg = "Databasfel";
                        return 0;
                    }
                    return ad.Ad_Id;
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


        public AdDetails GetAd(int adId, out string errormsg)
        {
            
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "EXEC dbo.GetAd @Ad_Id = @adId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("adId", SqlDbType.Int).Value = Convert.ToInt32(adId);
           
            
            AdDetails details = new AdDetails();
            SqlDataReader reader = null;
            errormsg = "";
            
             try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    details.Ad_Id = Convert.ToInt32(reader["Ad_Id"]);
                    details.Ad_Headline = reader["Ad_Headline"].ToString();
                    details.Ad_Content = reader["Ad_Content"].ToString();
                    details.Ad_ArticlePrice = Convert.ToInt32(reader["Ad_ArticlePrice"]);
                    details.Ad_AdPrice = Convert.ToInt32(reader["Price_AdPrice"]);
                    details.Ad_TypeId = Convert.ToInt32(reader["AdType_Id"]);
                    details.Ad_Type = reader["AdType_Text"].ToString();
                    details.Ad_AdvertiserId = Convert.ToInt32(reader["Advertiser_Id"]);
                    details.Ad_AdvertiserAddressId = Convert.ToInt32(reader["Advertiser_AddressId"]);
                    
                    if(details.Ad_TypeId == 2) {
                        details.Ad_AdvertiserBillingId = Convert.ToInt32(reader["Advertiser_BillingId"]);
                    }
                     
                }
                    reader.Close();
                    if(details.Ad_Id < 1) 
                    {
                        errormsg = "Databasfel";
                        return null;
                    }
                    return details;
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


        public List <AdDetails> GetAllAds(out string errormsg)
        {
            
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "EXEC dbo.GetAllAds;";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            
            List<AdDetails> detailsList = new List<AdDetails>();
            SqlDataReader reader = null;
            errormsg = "";
            
             try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();


                while (reader.Read())
                {   
                    AdDetails details = new AdDetails();
                    details.Ad_Id = Convert.ToInt32(reader["Ad_Id"]);
                    details.Ad_Headline = reader["Ad_Headline"].ToString();
                    details.Ad_Content = reader["Ad_Content"].ToString();
                    details.Ad_ArticlePrice = Convert.ToInt32(reader["Ad_ArticlePrice"]);
                    details.Ad_AdPrice = Convert.ToInt32(reader["Price_AdPrice"]);
                    details.Ad_TypeId = Convert.ToInt32(reader["AdType_Id"]);
                    details.Ad_Type = reader["AdType_Text"].ToString();
                    details.Ad_AdvertiserId = Convert.ToInt32(reader["Advertiser_Id"]);
                    details.Ad_AdvertiserAddressId = Convert.ToInt32(reader["Advertiser_AddressId"]);

                    detailsList.Add(details);
                     
                }
                    reader.Close();
                    return detailsList;
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


    }
}