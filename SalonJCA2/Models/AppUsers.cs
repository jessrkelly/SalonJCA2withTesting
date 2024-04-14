using Microsoft.AspNetCore.Identity;

namespace SalonJCA2.Models
{
    public class AppUsers : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
