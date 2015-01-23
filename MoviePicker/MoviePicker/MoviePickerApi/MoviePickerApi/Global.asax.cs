using System.Web;
using System.Web.Http;
using AutoMapper;
using TMDbLib.Objects.Movies;

namespace MoviePickerApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            Mapper.CreateMap<Movie, Models.Models.Movie>();
        }
    }
}