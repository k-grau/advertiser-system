using System;
using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{
    public class CompanyBilling
    {
         public int BillingId { get; set; }
        [Required(ErrorMessage = "Gatuadress/Postbox måste anges")]
        [Display(Name = "Gatuadress/Postbox")]
        public string BillingAddress { get; set; }
        [Required(ErrorMessage = "Postnummer måste anges")]
        [Range (10000, 99999, ErrorMessage ="Ange ett korrekt postnummer")]
        [Display(Name = "Postnummer")]
        public int BillingPostcode { get; set; }

        [Required(ErrorMessage = "Ort måste anges")]
        [Display(Name = "Ort")]
        public string BillingCity { get; set; }
    }
}