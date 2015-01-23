using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Repositories.Declarations;
using Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof (User))]
        public IHttpActionResult GetUser(int? id)
        {
            throw new NotImplementedException();
        }

        [Route("create")]
        [HttpPost]
        [ResponseType(typeof (User))]
        public IHttpActionResult CreateUser([FromBody] User user)
        {
            throw new NotImplementedException();
        }

        [Route("login")]
        [HttpPost]
        [ResponseType(typeof (User))]
        public IHttpActionResult Login([FromBody] User user)
        {
            throw new NotImplementedException();
        }

        private bool UserExists(string email)
        {
            throw new NotImplementedException();
        }
    }
}