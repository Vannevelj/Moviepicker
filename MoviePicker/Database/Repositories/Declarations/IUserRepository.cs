using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Users;

namespace Database.Repositories.Declarations
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserModel userModel);
        Task<IdentityUser> FindUserAsync(string username, string password);
    }
}