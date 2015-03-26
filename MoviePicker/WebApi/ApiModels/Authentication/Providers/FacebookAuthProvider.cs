using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;

namespace WebApi.ApiModels.Authentication.Providers
{
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        public override async Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
        }
    }
}