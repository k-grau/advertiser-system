using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;
using Advertisers.Models;
using Advertisers.DAL;


namespace Advertisers.Controllers
{
    public class AdsController : Controller
    {
       private CompanyDataHandler cdh;
       private AdDataHandler adh;
       private SubscriberDataHandler sdh; 
       public string svmTempSubscriber;
       string errormsg = "";
        string message = "";

        public AdsController(IConfiguration configuration) {
            sdh = new SubscriberDataHandler(configuration);
            cdh = new CompanyDataHandler(configuration);
            adh = new AdDataHandler(configuration);
        }
        

        
       [HttpGet]
        public IActionResult AdvertiserType(string Advertiser)
        {
            if(Advertiser == "Company")
            {
                return RedirectToAction("CompanyInformation", "Ads"); 
            }
             else if(Advertiser == "Subscriber") 
            {
                return RedirectToAction("SubscriberLogin", "Ads", new {error = ""}); 
            }
            return RedirectToAction("Index", "Home"); 
        }


        private int generateAdvertiserId(int identifier, int id) {
            int i = 1;
            int advertiserId = 0;
            while (i <= id) {
                i *= 10;
            }
            advertiserId = identifier * i + id; 
            return advertiserId;
        }


        [HttpGet]
        public IActionResult AdInformation(string Advertiser)
        {
            
            if(Advertiser == "Subscriber") 
            {
                @ViewBag.type = " Prenumerantannons.";
                @ViewBag.price = " 0 kronor.";

                
            }
             else if(Advertiser == "Company") 
            {
                @ViewBag.type = " Företagsannons.";
                @ViewBag.price = " 40 kronor.";
                
            } 
            return View(); 
        }


        [HttpPost]
        public IActionResult AdInformation(AdDetails ad)
        {
            int adId = 0;
            var advertiser = HttpContext.Session.GetString("advertiserType");
            
            if(advertiser == "subscriber") 
            {
                var svmSubscriber = this.HttpContext.Session.GetString("svmSubscriber");
                var svm = JsonConvert.DeserializeObject<SubscriberViewModel>(svmSubscriber);                
                ad.Ad_AdPriceId = 1;
                ad.Ad_TypeId = 1;
                ad.Ad_AdvertiserId = generateAdvertiserId(1, svm.SubscriberNo);
                ad.Ad_AdvertiserAddressId= svm.SubscriberNo;
                ad.Ad_AdvertiserBillingId = 0;


            } else if(advertiser == "company") 
            {

                var cvmCompany = this.HttpContext.Session.GetString("cvmCompany");
                var cvm = JsonConvert.DeserializeObject<CompanyViewModel>(cvmCompany);
                ad.Ad_AdPriceId = 2;
                ad.Ad_TypeId = 2;
                ad.Ad_AdvertiserId = generateAdvertiserId(2, cvm.Details.CompanyId);
                ad.Ad_AdvertiserAddressId = cvm.Contact.ContactId;
                ad.Ad_AdvertiserBillingId = cvm.Billing.BillingId;
            } 
                
            
            adId = adh.CreateAdd(ad, out string errormsg);
            if(adId < 0) 
            {
                ViewBag.error = "Kunde inte lägga till annons. Fel: " + errormsg;
                return View(); 
            }
            return RedirectToAction("CreatedAd", "Ads", new {adid = adId, adtypeId = ad.Ad_TypeId});
        }


        [HttpGet]
        public IActionResult CreatedAd(int adid, int adtypeId)
        {
            var advertiser = HttpContext.Session.GetString("advertiserType");
            AdViewModel adv = null;
            
            if(advertiser == "subscriber") 
            {
                var svmSubscriber = this.HttpContext.Session.GetString("svmSubscriber");
                var svm = JsonConvert.DeserializeObject<SubscriberViewModel>(svmSubscriber); 
               
                adv = new AdViewModel 
                {
                    Ad = adh.GetAd(adid, out errormsg),
                    Advertiser = svm.Firstname + " " + svm.Lastname,
                    SubscriberContact = svm.Contact,
                };
                
    
            } else if(advertiser == "company") 
            {
                var cvmCompany = this.HttpContext.Session.GetString("cvmCompany");
                var cvm = JsonConvert.DeserializeObject<CompanyViewModel>(cvmCompany);

                adv = new AdViewModel 
                {
                    Ad = adh.GetAd(adid, out errormsg),
                    Advertiser = cvm.Details.Name,
                    CompanyContact = cvm.Contact,
                    CompanyBilling = cvm.Billing,

                }; 
            }
            
            return View(adv);
        }






        [HttpGet]
        public  async Task<IActionResult> AdDetails(int adid, int adtypeId)
        {
            AdDetails adDetails = adh.GetAd(adid, out errormsg);
            int advertiserId = Convert.ToInt32(adDetails.Ad_AdvertiserId.ToString().Remove(0,1));
            SubscriberViewModel svm;
            CompanyViewModel cvm;
            AdViewModel adv = null;
            
            
            if(adtypeId == 1) 
            {
                svm = await sdh.GetSubscriber(advertiserId);
                
                adv = new AdViewModel 
                {
                    Ad = adDetails,
                    Advertiser = svm.Firstname + " " + svm.Lastname,
                    SubscriberContact = svm.Contact,
                };
    
            } else if(adtypeId == 2) 
            {
                cvm = cdh.GetFullCompanyDetails(advertiserId, adDetails.Ad_AdvertiserAddressId, 
                adDetails.Ad_AdvertiserBillingId,  out errormsg);

                adv = new AdViewModel 
                {
                    Ad = adh.GetAd(adid, out errormsg),
                    Advertiser = cvm.Details.Name,
                    CompanyContact = cvm.Contact,
                    CompanyBilling = cvm.Billing,
                }; 
            }
            return View(adv);
        }



        [HttpGet]
        public async Task<IActionResult> AdList()
        {
            List<AdDetails> adDetailsList = adh.GetAllAds(out errormsg);
            List<AdViewModel> advl = new List<AdViewModel>();
            AdViewModel adv; 
            List<SubscriberViewModel> svml;
            
            int id = 0;
            svml = await sdh.GetSubscribers();
            foreach(AdDetails a in adDetailsList) 
            {
               id = Convert.ToInt32(a.Ad_AdvertiserId.ToString().Remove(0,1));

                if(a.Ad_TypeId == 1) 
                {
                    foreach(SubscriberViewModel s in svml) 
                    {  
                        if(s.SubscriberNo == id) 
                        {
                            adv = new AdViewModel 
                            {
                                Advertiser = s.Firstname + " " + s.Lastname,
                                Ad = a 
                            };
                            advl.Add(adv);
                        }
                    }
                    
                }

                if(a.Ad_TypeId == 2) 
                {
                    adv = new AdViewModel 
                    {
                        Advertiser = cdh.GetCompanyName(id, errormsg),
                        Ad = a 
                    };
                    
                    advl.Add(adv);
                }
            }
            return View(advl);           
        }
      



        [HttpGet]
        public IActionResult SubscriberLogin(string error)
        {
            ViewBag.error = error;
            return View();
        }



        [HttpPost]
        public IActionResult SubscriberLogin(int subscriber)
        {
            return RedirectToAction("SubscriberInformation", "Ads", new {subscriberNo = subscriber});
           
        }


        [HttpGet]
        public IActionResult CompanyInformation()
        {
            ViewBag.error = errormsg;
            return View();
        }

        

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CompanyInformation(CompanyViewModel cvm)
        {
            ViewBag.error = errormsg;
            ViewBag.msg = message;
            

            cvm.Details.CompanyId = cdh.SetCompany(cvm.Details, out errormsg);

            if(cvm.Details.CompanyId == 0) 
            {
                ViewBag.error = "Något gick fel. Kan inte lägga till eller hitta företag. Felmeddelande: " +  errormsg;
                return View("CompanyInformation");
            }
            cvm.Contact.ContactId = cdh.SetCompanyContact(cvm.Details.CompanyId, cvm.Contact, out errormsg);

            if(cvm.Contact.ContactId == 0) 
            {
                ViewBag.error = "Något gick fel. Kan inte lägga till eller hitta företagets adress. Felmeddelande: " +  errormsg;
                return View("CompanyInformation");
            }
            cvm.Billing.BillingId = cdh.SetCompanyBilling(cvm.Details.CompanyId, cvm.Billing, out errormsg);
            
            if(cvm.Billing.BillingId == 0) 
            {
                ViewBag.error = "Något gick fel. Kan inte lägga till eller hitta företagets fakutraadress. Felmeddelande: " +  errormsg;
                return View("CompanyInformation");
            }

            var cvmJSON = JsonConvert.SerializeObject(cvm);
            HttpContext.Session.SetString("cvmCompany", cvmJSON);
            HttpContext.Session.SetString("advertiserType", "company");
            return RedirectToAction("AdInformation", new {Advertiser = "Company" });
        }



        [HttpGet]
        public async Task<IActionResult> SubscriberInformation(int subscriberNo)
        {
            ViewBag.error = errormsg;
            SubscriberViewModel svm;

            svm = await sdh.GetSubscriber(subscriberNo);
            if(svm == null) 
            {
               return RedirectToAction("SubscriberLogin", new{error = "Prenumerationsnummer saknas. Försök igen."}); 
            } 
           
            var svmJSON = JsonConvert.SerializeObject(svm);
            HttpContext.Session.SetString("svmTempSubscriber", svmJSON);
            
            return View(svm);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task <IActionResult> SubscriberInformation (SubscriberViewModel svm)
        {
            ViewBag.error = errormsg;
            int attributesToChange = 7;
            SubscriberViewModel svmTemp = new SubscriberViewModel();
            var svmTempSubscriber = this.HttpContext.Session.GetString("svmTempSubscriber");
            svmTemp = JsonConvert.DeserializeObject<SubscriberViewModel>(svmTempSubscriber);

            var svmJSON = JsonConvert.SerializeObject(svm);
            HttpContext.Session.SetString("svmSubscriber", svmJSON);
            HttpContext.Session.SetString("advertiserType", "subscriber");


            if(svmTemp.PersonalNo == svm.PersonalNo) 
            {
                svm.PersonalNo = 0;
                attributesToChange--;
            } 
            
            if(svmTemp.Firstname == svm.Firstname) 
            {
                svm.Firstname = null;
                attributesToChange--;
            } 
            
            if(svmTemp.Lastname == svm.Lastname) 
            {
                svm.Lastname = null;
                attributesToChange--;
            } 
            
            if(svmTemp.Contact.Address == svm.Contact.Address) 
            {
                svm.Contact.Address = null;
                attributesToChange--;
            } 
            
            if(svmTemp.Contact.Postcode == svm.Contact.Postcode) 
            {
                svm.Contact.Postcode = 0;
                attributesToChange--;
            } 
            
            if(svmTemp.Contact.City == svm.Contact.City) 
            {
                svm.Contact.City = null;
                attributesToChange--;
            } 
            
            if(svmTemp.Contact.Phone == svm.Contact.Phone) 
            {
                svm.Contact.Phone = null;
                attributesToChange--;
            } 
            
          
            if(attributesToChange > 0) 
            {
                int status = await sdh.PutSubscriber(svm);
                if(status != 200) 
                {
                    ViewBag.error = "Något gick fel. Kunde inte uppdatera uppgifterna.";
                    return View();
                }
            }

            return RedirectToAction("AdInformation", new {Advertiser = "Subscriber" });
        }
    }
}