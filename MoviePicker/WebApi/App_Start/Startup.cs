using System;
using System.Configuration;
using System.Web.Http;
using Database.DatabaseModels;
using Database.Repositories;
using Database.Repositories.Declarations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using WebApi.ApiModels.Authentication;
using WebApi.ApiModels.Authentication.Providers;

namespace WebApi.App_Start
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; set; }
        public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; set; }
        public static FacebookAuthenticationOptions FacebookOptions { get; set; }

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
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            // Google external login
            GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "unknown",
                ClientSecret = "unknown",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(GoogleAuthOptions);

            // Facebook external login
            FacebookOptions = new FacebookAuthenticationOptions
            {
                AppId = ConfigurationManager.AppSettings["fb_app_id"],
                AppSecret = ConfigurationManager.AppSettings["fb_app_secret"],
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(FacebookOptions);

            WebApiConfig.Register(configuration);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration);
        }
    }
}