using System;
using System.Web.Http;
using System.Web.Http.Description;
using MoviePickerApi.Models.ApiParameters;
using TMDbLib.Objects.Movies;

namespace MoviePickerApi.Controllers
{
    [RoutePrefix("api/movies")]
    public class MovieController : BaseController
    {
        public MovieController()
        {
            Client.GetConfig();
        }

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof (Movie))]
        public IHttpActionResult GetMovieDetails(int id)
        {
            var movie = Client.GetMovie(id);
            movie.PosterPath = Client.GetImageUrl("original", movie.PosterPath).ToString();
            return Ok(movie);
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