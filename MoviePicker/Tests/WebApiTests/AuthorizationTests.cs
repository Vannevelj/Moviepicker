using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Database.DatabaseModels;
using Database.Repositories;
using Database.Repositories.Declarations;
using Effort;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestUtilities;
using WebApi.App_Start;
using WebApi.Controllers;

namespace Tests.WebApiTests
{
    [TestClass]
    public class AuthorizationTests
    {
        private AccountController _accountController;
        private MoviepickerContext _context;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            var configuration = new HttpConfiguration();
            _context = new MoviepickerContext(DbConnectionFactory.CreateTransient());
            _userRepository = new UserRepository(_context);
            _accountController = new AccountController(_userRepository) { Configuration = configuration };
            TestStartupConfiguration.Context = _context;
            TestStartupConfiguration.UserRepository = _userRepository;
            TestStartupConfiguration.HttpConfiguration = configuration;
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithoutRegisteredUser_ReturnsBadRequest()
        {
            var user = TestDataProvider.GetUserModel();

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                })).PostAsync();

                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithRegisteredUser_ReturnsToken()
        {
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                })).PostAsync();

                response.IsSuccessStatusCode.Should().BeTrue();
                (await response.Content.ReadAsStringAsync()).Should().NotBeNullOrWhiteSpace();
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithInvalidCredentials_ReturnsBadRequest()
        {
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password + "this can't work"),
                    new KeyValuePair<string, string>("grant_type", "password")
                })).PostAsync();

                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        private class TestStartupConfiguration : Startup
        {
            public static MoviepickerContext Context;
            public static IUserRepository UserRepository;
            public static HttpConfiguration HttpConfiguration;

            public override HttpConfiguration GetInjectionConfiguration()
            {
                var container = new UnityContainer();
                container.RegisterInstance(UserRepository);
                container.RegisterInstance(Context);
                HttpConfiguration.DependencyResolver = new UnityConfig(container);
                return HttpConfiguration;
            }
        }
    }
}