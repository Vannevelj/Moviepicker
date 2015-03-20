using System;
using System.Web.Http;
using Database.DatabaseModels;
using Database.Repositories;
using Database.Repositories.Declarations;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using WebApi.ApiModels.Authentication;

namespace WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Web API configuration and services
            var container = new UnityContainer();
            var context = new MoviepickerContext();
            var userRepository = new UserRepository(context);
            var movieRepository = new MovieRepository(context);
            container.RegisterInstance<IUserRepository>(userRepository);
            container.RegisterInstance<IMovieRepository>(movieRepository);
            container.RegisterInstance(context);
            config.DependencyResolver = new UnityConfig(container);

            // OAuth configuration
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), 
                Provider = new SimpleAuthorizationServerProvider(userRepository)
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll); 
            app.UseWebApi(config);    
        }
    }
}