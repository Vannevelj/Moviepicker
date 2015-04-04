using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DatabaseModels;
using Database.Repositories.Declarations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Users;
using Models.Users.Authorization;
using Models.Utilities;

namespace Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MoviepickerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(MoviepickerContext context)
        {
            _context = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public async Task<IdentityResult> RegisterUserAsync(UserModel userModel)
        {
            var user = new ApplicationUser(userModel.Username);
            return await _userManager.CreateAsync(user, userModel.Password);
        }

        public async Task<ApplicationUser> FindUserAsync(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

        public async Task<ClientApplication> FindClientAsync(string clientId)
        {
            return await _context.ClientApplications.FindAsync(clientId);
        }

        public async Task<bool> TryAddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var existingToken = _context.RefreshTokens.SingleOrDefault(x => x.Subject == refreshToken.Subject && x.ClientApplicationId == refreshToken.ClientApplicationId);

            if (existingToken != null)
            {
                await TryRemoveRefreshTokenAsync(existingToken);
            }

            _context.RefreshTokens.Add(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TryRemoveRefreshTokenAsync(string refreshTokenId)
        {
            var existingToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> TryRemoveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshTokenAsync(string refreshTokenId)
        {
            return await _context.RefreshTokens.FindAsync(refreshTokenId);
        }

        public IEnumerable<RefreshToken> GetRefreshTokens()
        {
            return _context.RefreshTokens;
        }

        public async Task<bool> TryCreateClientAsync(ClientApplication clientApplication)
        {
            var existingClient = await _context.ClientApplications.FindAsync(clientApplication.Id);
            if (existingClient != null)
            {
                return false;
            }

            clientApplication.Secret = AuthorizationHelpers.GetHash(clientApplication.Secret ?? string.Empty);
            _context.ClientApplications.Add(clientApplication);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ApplicationUser> FindUserAsync(UserLoginInfo loginInfo)
        {
            return await _userManager.FindAsync(loginInfo);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo)
        {
            return await _userManager.AddLoginAsync(userId, loginInfo);
        }
    }
}