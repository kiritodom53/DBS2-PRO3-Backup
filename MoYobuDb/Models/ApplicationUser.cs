using Microsoft.AspNetCore.Identity;

namespace MoYobuDb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImage { get; set; }
    }
}