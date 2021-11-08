using System;
using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{
    public class CompanyContact
    {
         public int ContactId { get; set; }
        [Required(ErrorMessage = "Gatuadress måste anges")]
        [Display(Name = "Gatuadress")]
        public string ContactAddress { get; set; }
        [Required(ErrorMessage = "Postnummer måste anges")]
        [Range (10000, 99999, ErrorMessage ="Ange ett korrekt postnummer")]
        [Display(Name = "Postnummer")]
        public int ContactPostcode { get; set; }
        [Required(ErrorMessage = "Ort måste anges")]
        [Display(Name = "Ort")]
        public string ContactCity { get; set; }
        [Required(ErrorMessage = "Telefonnummer måste anges")]
        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"0([-\s]?\d){6,10}", ErrorMessage = "Ange giltigt telefonnummer")]
        public string ContactPhone { get; set; }  
    }
}