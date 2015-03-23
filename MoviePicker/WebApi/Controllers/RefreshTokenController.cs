using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Repositories.Declarations;
using Models.Users.Authorization;

namespace WebApi.Controllers
{
    [RoutePrefix("api/refreshtokens")]
    public class RefreshTokenController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public RefreshTokenController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("")]
        [HttpGet]
        //[Authorize(Roles = "Admin")] TODO: implement roles
        [ResponseType(typeof (IEnumerable<RefreshToken>))]
        public IHttpActionResult Get()
        {
            return Ok(_userRepository.GetRefreshTokens());
        }

        [Route("")]
        [HttpDelete]
        //[Authorize(Roles = "Admin")] TODO: implement roles
        [ResponseType(typeof (void))]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _userRepository.TryRemoveRefreshTokenAsync(tokenId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Token does not exist.");
        }
    }
}