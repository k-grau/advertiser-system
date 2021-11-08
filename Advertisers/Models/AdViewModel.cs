using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{

    public class AdViewModel
    {
        public AdDetails Ad { get; set; }
        [Display(Name = "Annonsör")]
        public string Advertiser {get; set; }
        public SubscriberContact SubscriberContact {get; set;}
        public CompanyContact CompanyContact {get; set;}
        public CompanyBilling CompanyBilling { get; set; }
        
    }
}