using System;
using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{
    public class SubscriberViewModel
    {
        [Display(Name = "Prenumerationsnummer")]
         public int SubscriberNo { get; set; }
        [Required(ErrorMessage = "Personnummer får inte vara tomt")]
        [Range (190000000000, 201912319999, ErrorMessage ="Ange tio siffror, format ååååmmddnnnn")]
        [Display(Name = "Personnummer")]
        public long PersonalNo { get; set; }
        [Required(ErrorMessage = "Förnamn får inte vara tomt")]
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Efternamn får inte vara tomt")]
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; }
        public SubscriberContact Contact { get; set; }
    }
}