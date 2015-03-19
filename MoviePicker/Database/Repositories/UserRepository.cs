using System.Threading.Tasks;
using Database.DatabaseModels;
using Database.Repositories.Declarations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Users;

namespace Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviepickerContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(MoviepickerContext context)
        {
            _context = context;
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser(userModel.Username);
            return await _userManager.CreateAsync(user, userModel.Password);
        }

        public async Task<IdentityUser> FindUser(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

        public void Dispose()
        {
            _context.Dispose();
            _userManager.Dispose();
        }
    }
}