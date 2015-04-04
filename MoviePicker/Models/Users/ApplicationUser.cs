using Microsoft.AspNet.Identity.EntityFramework;

namespace Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string username) : base(username)
        {
        }
    }
}