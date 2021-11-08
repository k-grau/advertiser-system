using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Advertisers.Models;



namespace Advertisers.DAL
{
    public class SubscriberDataHandler
    {

        private string apiBaseUrl = "";
        

        public SubscriberDataHandler(IConfiguration configuration) {
            apiBaseUrl = configuration.GetValue<string>("WebAPIBaseUrl");
        }

        
    
       
        [HttpPut]
        public async Task <int> PutSubscriber(SubscriberViewModel svm) 
         {
             int status = 0;
               
                using (var httpClientHandler = new HttpClientHandler())
                {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    var response = await client.PutAsJsonAsync(apiBaseUrl + svm.SubscriberNo + "?update=true", svm);
                    status = (int)response.StatusCode;  
                }
            }
            return status;
         }


        [HttpGet]
        public async Task<SubscriberViewModel> GetSubscriber(int subscriberNo)
        {
            SubscriberViewModel svm = new SubscriberViewModel();
            
            using (var httpClientHandler = new HttpClientHandler())
            {
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    using (var response = await client.GetAsync(apiBaseUrl + subscriberNo))
                    {   
                        if((int)response.StatusCode != 200) {
                            return null;
                        }
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        
                        svm = JsonConvert.DeserializeObject<SubscriberViewModel>(apiResponse);
                    }
                }
                
            }
            return svm;
        }



        [HttpGet]
        public async Task<List<SubscriberViewModel>> GetSubscribers()
        {
            List<SubscriberViewModel> svml = new List<SubscriberViewModel>();
            
            using (var httpClientHandler = new HttpClientHandler())
            {
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    using (var response = await client.GetAsync(apiBaseUrl))
                    {   
                        if((int)response.StatusCode != 200) {
                            return null;
                        }
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        
                        svml = JsonConvert.DeserializeObject<List<SubscriberViewModel>>(apiResponse);
                    }
                }
                
            }
            return svml;
        }
    }
}