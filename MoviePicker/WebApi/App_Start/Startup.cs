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
        public virtual HttpConfiguration GetInjectionConfiguration()
        {
            var configuration = new HttpConfiguration();
            var container = new UnityContainer();
            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<IMovieRepository, MovieRepository>(new TransientLifetimeManager());
            container.RegisterInstance(new MoviepickerContext());
            configuration.DependencyResolver = new UnityConfig(container);
            return configuration;
        }

        public void Configuration(IAppBuilder app)
        {
            var configuration = GetInjectionConfiguration();
            var injectedUserRepository = (IUserRepository) configuration.DependencyResolver.GetService(typeof (IUserRepository));

            // OAuth configuration
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(injectedUserRepository),
                RefreshTokenProvider = new SimpleRefreshTokenProvider(injectedUserRepository)
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            WebApiConfig.Register(configuration);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration);
        }
    }
}