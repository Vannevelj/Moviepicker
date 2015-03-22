﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.Users;
using Models.Users.Authorization;

namespace Database.Repositories.Declarations
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserModel userModel);
        Task<IdentityUser> FindUserAsync(string username, string password);
        Task<ClientApplication> FindClientAsync(string clientId);
        Task<bool> TryAddRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> TryRemoveRefreshTokenAsync(string refreshTokenId);
        Task<bool> TryRemoveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshTokenAsync(string refreshTokenId);
        IEnumerable<RefreshToken> GetRefreshTokens();
        Task<bool> TryCreateClientAsync(ClientApplication clientApplication);
    }
}