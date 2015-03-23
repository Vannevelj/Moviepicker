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
using Newtonsoft.Json.Linq;
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
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithRegisteredUser_ReturnsToken()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                response.IsSuccessStatusCode.Should().BeTrue();
                (await response.Content.ReadAsStringAsync()).Should().NotBeNullOrWhiteSpace();
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithInvalidCredentials_ReturnsBadRequest()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password + "this can't work"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithInvalidClientId_ReturnsBadRequest()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password + "this can't work"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "this can't work"),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_WithInvalidClientSecret_ReturnsBadRequest()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", "this can't work")
                })).PostAsync();

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetToken_ForJavascriptApp_WithoutClientSecret_ReturnsToken()
        {
            var clientApplication = TestDataProvider.GetJavascriptClientApplication();
            await _userRepository.TryCreateClientAsync(clientApplication);
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", clientApplication.Id)
                })).PostAsync();

                response.IsSuccessStatusCode.Should().BeTrue();
                (await response.Content.ReadAsStringAsync()).Should().NotBeNullOrWhiteSpace();
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetRefreshToken_WithValidCredentials_ReturnsNewToken()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                var responseContent = JObject.Parse(await response.Content.ReadAsStringAsync());
                var initialRefreshToken = responseContent["refresh_token"].Value<string>();

                response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret),
                    new KeyValuePair<string, string>("refresh_token", initialRefreshToken)
                })).PostAsync();

                responseContent = JObject.Parse(await response.Content.ReadAsStringAsync());
                var newRefreshToken = responseContent["refresh_token"].Value<string>();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                newRefreshToken.Should().NotBe(initialRefreshToken);
            }
        }

        [TestMethod]
        [TestCategory("Unit_AUTHORIZATION")]
        public async Task GetRefreshToken_WithInvalidExistingRefreshToken_ReturnsBadRequest()
        {
            var testClientApplication = await InsertClientApplication();
            var user = TestDataProvider.GetUserModel();
            await _accountController.RegisterAsync(user);

            using (var server = TestServer.Create<TestStartupConfiguration>())
            {
                var response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user.Username),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret)
                })).PostAsync();

                var responseContent = JObject.Parse(await response.Content.ReadAsStringAsync());
                var initialRefreshToken = responseContent["refresh_token"].Value<string>();

                response = await server.CreateRequest("/token").And(x => x.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("client_id", testClientApplication.ClientApplication.Id),
                    new KeyValuePair<string, string>("client_secret", testClientApplication.ClientSecret),
                    new KeyValuePair<string, string>("refresh_token", initialRefreshToken + "this can't work")
                })).PostAsync();

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        private async Task<TestDataProvider.TestClientApplication> InsertClientApplication()
        {
            var clientApplication = TestDataProvider.GetNativeClientApplication();
            clientApplication.ClientApplication.Secret = clientApplication.ClientSecret;
            await _userRepository.TryCreateClientAsync(clientApplication.ClientApplication);
            return clientApplication;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
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