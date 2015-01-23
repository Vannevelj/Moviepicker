using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviePickerApi.Models;
using MoviePickerApi.Models.Models;

namespace MoviePickerApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : BaseController
    {
        private readonly MoviePickerContext _db = new MoviePickerContext();

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof (User))]
        public async Task<IHttpActionResult> GetUser(int? id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("create")]
        [HttpPost]
        [ResponseType(typeof (User))]
        public async Task<IHttpActionResult> CreateUser([FromBody] User user)
        {
            if (UserExists(user.Email))
            {
                return Conflict();
            }

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            user.Password = string.Empty;
            return Ok(user);
        }

        [Route("login")]
        [HttpPost]
        [ResponseType(typeof (User))]
        public async Task<IHttpActionResult> Login([FromBody] User user)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (dbUser == null || dbUser.Password != user.Password)
            {
                return NotFound();
            }

            return Ok(dbUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string email)
        {
            return _db.Users.FirstOrDefault(e => e.Email == email) != null;
        }
    }
}