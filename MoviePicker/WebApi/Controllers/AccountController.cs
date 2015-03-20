using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Repositories.Declarations;
using Microsoft.AspNet.Identity;
using Models.Users;

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
    }
}