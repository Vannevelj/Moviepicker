using System;
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

        public DateTime DateOfBirth { get; set; }
        public bool SubscribedToNewsletter { get; set; }
    }
}