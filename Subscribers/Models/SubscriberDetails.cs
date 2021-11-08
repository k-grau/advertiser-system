
namespace Subscribers.Models
{
    public class SubscriberDetails
    {
        public int SubscriberNo { get; set; }
        public long PersonalNo { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ContactDetails Contact { get; set; }       
    }
}