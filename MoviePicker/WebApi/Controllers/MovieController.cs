using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Repositories.Declarations;
using Models.Movies;
using WebApi.ApiModels.ApiParameters;

namespace WebApi.Controllers
{
    [RoutePrefix("api/movies")]
    public class MovieController : ApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        public MovieController(IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof (Movie))]
        public IHttpActionResult GetMovieDetails(int id)
        {
            throw new NotImplementedException();
        }

        [Route("like")]
        [HttpPost]
        public IHttpActionResult LikeMovie([FromBody] MovieActionParameters parameters)
        {
            throw new NotImplementedException();
        }

        [Route("dislike")]
        [HttpPost]
        public IHttpActionResult DislikeMovie([FromBody] MovieActionParameters parameters)
        {
            throw new NotImplementedException();
        }

        [Route("~/api/users/{userId:int}/movies")]
        [HttpGet]
        public IHttpActionResult GetMovie(int userId)
        {
            throw new NotImplementedException();
        }
    }
}