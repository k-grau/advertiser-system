using System;
using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{
    public class SubscriberContact
    {
        [Required(ErrorMessage = "Adress får inte vara tom")]
        [Display(Name = "Adress")]
        public string Address {get; set;}
        
        [Required(ErrorMessage = "Postnummer får inte vara tomt")]
        [Range (10000, 99999, ErrorMessage ="Ange ett korrekt postnummer")]
        [Display(Name = "Postnummer")]
        public int Postcode { get; set; }
        [Required(ErrorMessage = "Ort får inte vara tomt")]
        [Display(Name = "Ort")]
        public string City {get; set;}
        [Required(ErrorMessage = "Telefonnummer får inte vara tomt")]
        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"0([-\s]?\d){6,10}", ErrorMessage = "Ange giltigt telefonnummer")]
        public string Phone { get; set; }  
    }
}