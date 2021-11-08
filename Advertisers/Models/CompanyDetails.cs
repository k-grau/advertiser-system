using System;
using System.ComponentModel.DataAnnotations;



namespace Advertisers.Models
{
    public class CompanyDetails
    {
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Organisationsnummer måste anges")]
        [Range (111, 99999999999, ErrorMessage ="Ange ett korrekt organisationsnummer")]
        [Display(Name = "Organisationsnummer")]
        public int OrgNo { get; set; }
        [Required(ErrorMessage = "Företagsnamn måste anges")]
        [Display(Name = "Företagsnamn")]
        public string Name { get; set; }
    }
}