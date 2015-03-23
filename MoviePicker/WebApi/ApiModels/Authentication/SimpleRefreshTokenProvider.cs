using System;
using System.Threading.Tasks;
using Database.Repositories.Declarations;
using Microsoft.Owin.Security.Infrastructure;
using Models.Users.Authorization;
using Models.Utilities;

namespace WebApi.ApiModels.Authentication
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IUserRepository _userRepository;

        public SimpleRefreshTokenProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("N");
            var refreshTokenLifetime = context.OwinContext.Get<string>("as:clientRefreshTokenLifetime");
            var token = new RefreshToken
            {
                Id = AuthorizationHelpers.GetHash(refreshTokenId),
                ClientApplicationId = clientId,
                Subject = context.Ticket.Identity.Name,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifetime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedAt;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresAt;

            token.ProtectedTicket = context.SerializeTicket();

            if (await _userRepository.TryAddRefreshTokenAsync(token))
            {
                context.SetToken(refreshTokenId);
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            var hashedTokenId = AuthorizationHelpers.GetHash(context.Token);

            var refreshToken = await _userRepository.FindRefreshTokenAsync(hashedTokenId);
            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                await _userRepository.TryRemoveRefreshTokenAsync(hashedTokenId);
            }
        }
    }
}