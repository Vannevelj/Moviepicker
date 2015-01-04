using System.Web.Http;

namespace MoviePickerApi
{
    using AutoMapper;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            Mapper.CreateMap<TMDbLib.Objects.Movies.Movie, Shared.Models.Movie>();
        }
    }
}
