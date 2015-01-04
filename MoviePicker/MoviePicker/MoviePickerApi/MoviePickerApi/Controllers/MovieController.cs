using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MoviePickerApi.Controllers
{
    using System;
    using Shared.Models.ApiParameters;

    [RoutePrefix("api/movies")]
    public class MovieController : BaseController
    {
        public MovieController()
        {
            Client.GetConfig();
        }

        [Route("~/api/genres")]
        [HttpGet]
        [ResponseType(typeof (IEnumerable<Movie>))]
        public IHttpActionResult GetGenres()
        {
            return Ok(Client.GetGenres());
        }

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(Movie))]
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
