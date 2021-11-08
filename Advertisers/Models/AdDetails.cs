using System.ComponentModel.DataAnnotations;


namespace Advertisers.Models
{
    public class AdDetails
    {
        [Display(Name = "Annonsnummer")]
        public int Ad_Id { get; set; }
        [Required(ErrorMessage = "Rubrik måste anges")]
        [MaxLength(30, ErrorMessage = "Du kan ange max 30 tecken")]
        [Display(Name = "Rubrik")]
         public string Ad_Headline { get; set; }
        [Required(ErrorMessage = "Annonstext måste anges")]
        [Display(Name = "Annonstext")]
        [MaxLength(200, ErrorMessage = "Du kan ange max 200 tecken")]
        public string Ad_Content { get; set; }
        [Required(ErrorMessage = "Pris måste anges")]
        [Display(Name = "Varans pris")]
        public int Ad_ArticlePrice { get; set; }
        public int Ad_AdPriceId { get; set; }
        [Display(Name = "Annonspris")]
        public int Ad_AdPrice { get; set; }
        public int Ad_TypeId { get; set; }
        [Display(Name = "Annonstyp")]
        public string Ad_Type { get; set; }
        public int Ad_AdvertiserId { get; set; }
        public int Ad_AdvertiserAddressId { get; set; }
        public int Ad_AdvertiserBillingId { get; set; }
    }
}