using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Repositories.Declarations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Models.Users;
using Models.Users.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.ApiModels.Authentication.Results;
using WebApi.App_Start;

namespace WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // POST api/account/register
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        [ResponseType(typeof (void))] //TODO: return userinfo?
        public async Task<IHttpActionResult> RegisterAsync(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identityResult = await _userRepository.RegisterUserAsync(userModel);
            var errorResult = GetErrorResult(identityResult);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // GET api/account/externallogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [HttpGet]
        [Route("externallogin")]
        [ResponseType(typeof (void))]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, Request);
            }

            var redirectUriValidationResult = await ValidateClientAndRedirectUriAsync();
            if (!redirectUriValidationResult.Item1)
            {
                return BadRequest(redirectUriValidationResult.Item2);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != null)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, Request);
            }

            var user = await _userRepository.FindUserAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));
            var hasRegistered = user != null;

            var redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                redirectUriValidationResult.Item2,
                externalLogin.ExternalAccessToken,
                externalLogin.LoginProvider,
                hasRegistered, externalLogin.Username);
            return Redirect(redirectUri);
        }

        // POST api/account/registerexternal
        [AllowAnonymous]
        [Route("registerexternal")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessTokenAsync(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid provider or external access token");
            }

            var user = await _userRepository.FindUserAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.UserId));
            if (user != null)
            {
                return BadRequest("External user is already registered");
            }

            user = new IdentityUser(model.Username);
            var result = await _userRepository.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var externalLoginInfo = new ExternalLoginInfo
            {
                DefaultUserName = model.Username,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.UserId)
            };

            result = await _userRepository.AddLoginAsync(user.Id, externalLoginInfo.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(GetLocalAccessTokenResponse(model.Username));
        }

        // GET api/account/obtainlocalaccesstoken
        [AllowAnonymous]
        [HttpGet]
        [Route("obtainlocalaccesstoken")]
        public async Task<IHttpActionResult> ObtainLocalAccessTokenAsync(string provider, string externalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent.");
            }

            var verifiedAccessToken = await VerifyExternalAccessTokenAsync(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid provider or external access token.");
            }

            var user = await _userRepository.FindUserAsync(new UserLoginInfo(provider, verifiedAccessToken.UserId));
            if (user == null)
            {
                return BadRequest("External user is not registered");
            }

            return Ok(GetLocalAccessTokenResponse(user.UserName));
        }

        private JObject GetLocalAccessTokenResponse(string username)
        {
            var tokenExpiration = TimeSpan.FromDays(1);
            var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim("role", "user"));

            var authenticationProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
            };

            var ticket = new AuthenticationTicket(identity, authenticationProperties);
            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            return new JObject(
                new JProperty("username", username),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds),
                new JProperty(".issued", ticket.Properties.IssuedUtc),
                new JProperty(".expires", ticket.Properties.ExpiresUtc));
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessTokenAsync(string provider, string accessToken)
        {
            Uri tokenVerificationEndpoint;
            if (provider == "Facebook")
            {
                var applicationToken = ConfigurationManager.AppSettings["fb_app_token"];
                tokenVerificationEndpoint = new Uri(string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, applicationToken));
            }
            else if (provider == "Google")
            {
                tokenVerificationEndpoint = new Uri(string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken));
            }
            else
            {
                return null;
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(tokenVerificationEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<JObject>(content);
                    var parsedToken = new ParsedExternalAccessToken();

                    if (provider == "Facebook")
                    {
                        parsedToken.UserId = jObject["data"]["user_id"].Value<string>();
                        parsedToken.ApplicationId = jObject["data"]["app_id"].Value<string>();

                        if (!Startup.FacebookOptions.AppId.Equals(parsedToken.ApplicationId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return null;
                        }
                    }
                    else if (provider == "Google")
                    {
                        parsedToken.UserId = jObject["user_id"].Value<string>();
                        parsedToken.ApplicationId = jObject["audience"].Value<string>();

                        if (!Startup.GoogleAuthOptions.ClientId.Equals(parsedToken.ApplicationId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return null;
                        }
                    }

                    return parsedToken;
                }

                return null;
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        /// <summary>
        ///     Returns false if validation failed combined with a specific reason.
        ///     Returns true if validation succeeded combined with a redirect uri.
        /// </summary>
        /// <returns></returns>
        private async Task<Tuple<bool, string>> ValidateClientAndRedirectUriAsync()
        {
            var redirectUriString = GetQueryString(Request, "redirect_uri");
            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return new Tuple<bool, string>(false, "redirect_uri is required.");
            }

            Uri uri;
            if (Uri.TryCreate(redirectUriString, UriKind.Absolute, out uri))
            {
                return new Tuple<bool, string>(false, "redirect_uri is invalid.");
            }

            var clientId = GetQueryString(Request, "client_id");
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return new Tuple<bool, string>(false, "client_id is required.");
            }

            var client = await _userRepository.FindClientAsync(clientId);
            if (client == null)
            {
                return new Tuple<bool, string>(false, string.Format("client_id '{0}' is not registered.", clientId));
            }

            if (!client.AllowedOrigin.Equals(uri.GetLeftPart(UriPartial.Authority), StringComparison.InvariantCultureIgnoreCase))
            {
                return new Tuple<bool, string>(false, string.Format("The given URL is not allowed by client id '{0}' configuration.", clientId));
            }

            return new Tuple<bool, string>(true, uri.AbsoluteUri);
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
            {
                return string.Empty;
            }

            var match = queryStrings.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase));
            if (string.IsNullOrWhiteSpace(match.Value))
            {
                return string.Empty;
            }

            return match.Value;
        }
    }
}