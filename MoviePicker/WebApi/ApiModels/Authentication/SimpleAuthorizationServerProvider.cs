using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.Repositories.Declarations;
using Microsoft.Owin.Security.OAuth;

#pragma warning disable 1998

namespace WebApi.ApiModels.Authentication
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider, IDisposable
    {
        private readonly IUserRepository _userRepository;

        public SimpleAuthorizationServerProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

            var user = _userRepository.FindUser(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            context.Validated();
        }
    }
}