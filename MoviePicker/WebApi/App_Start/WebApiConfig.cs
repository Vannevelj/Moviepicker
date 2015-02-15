using System.Web.Http;
using Database.Repositories;
using Database.Repositories.Declarations;
using Microsoft.Practices.Unity;
using WebApi.App_Start;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovieRepository, MovieRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityConfig(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }
    }
}