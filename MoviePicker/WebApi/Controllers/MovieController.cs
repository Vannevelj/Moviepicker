using System;
using System.Web.Http;
using System.Web.Http.Description;
using Models.Movies;
using WebApi.ApiModels.ApiParameters;

namespace WebApi.Controllers
{
    [RoutePrefix("api/movies")]
    public class MovieController
    {
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