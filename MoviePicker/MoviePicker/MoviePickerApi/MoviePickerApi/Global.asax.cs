using System.Web;
using System.Web.Http;
using AutoMapper;
using Models.Movies;

namespace MoviePickerApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            Mapper.CreateMap<Movie, Movie>();
        }
    }
}