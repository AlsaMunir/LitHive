using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserProfile : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        //adding country field for claim
        [NotAllowedAttributes(new char[] { ' ' })]
        public string Country { get; set; }
    }
}
