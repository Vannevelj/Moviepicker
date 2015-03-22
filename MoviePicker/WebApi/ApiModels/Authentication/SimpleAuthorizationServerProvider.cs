using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.Repositories.Declarations;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Models.Users.Authorization;
using Models.Utilities;

namespace WebApi.ApiModels.Authentication
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserRepository _userRepository;

        public SimpleAuthorizationServerProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId, clientSecret;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (clientId == null)
            {
                context.SetError("client_id_not_found", "ClientID should be sent.");
                return;
            }

            var clientApplication = await _userRepository.FindClientAsync(clientId);
            if (clientApplication == null)
            {
                context.SetError("invalid_client_id", string.Format("Client '{0}' is not registered in the system.", clientId));
                return;
            }

            // Only native apps are supposed to contain a secret
            // Javascript apps cannot store the secret safely anyway
            if (clientApplication.ApplicationType == ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("client_secret_not_found", "Client secret should be sent.");
                    return;
                }

                if (clientApplication.Secret != AuthorizationHelpers.GetHash(clientSecret))
                {
                    context.SetError("invalid_client_secret", "Client secret is invalid");
                    return;
                }
            }

            if (!clientApplication.IsActive)
            {
                context.SetError("client_inactive", "Client is inactive");
                return;
            }

            context.OwinContext.Set("as:clientAllowedOrigin", clientApplication.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifetime", clientApplication.RefreshTokenLifeTime);
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = await _userRepository.FindUserAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var authenticationProperties = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "as:client_id", context.ClientId ?? string.Empty },
                { "username", context.UserName }
            });
            var ticket = new AuthenticationTicket(identity, authenticationProperties);
            context.Validated(ticket);
        }

        public override async Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
        }
    }
}