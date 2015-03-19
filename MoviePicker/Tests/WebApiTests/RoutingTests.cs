using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;
using WebApi;
using WebApi.Controllers;

namespace Tests.WebApiTests
{
    [TestClass]
    public class RoutingTests
    {
        private HttpConfiguration _configuration;

        [TestInitialize]
        public void Initialize()
        {
            _configuration = new HttpConfiguration();
            WebApiConfig.Register(_configuration);
            _configuration.EnsureInitialized();
        }

        [TestMethod]
        [TestCategory("Unit_ROUTING")]
        public void RegisterAccount_CallsAppropriateMethod()
        {
            const string route = "/api/account/register";
            RouteAssert.HasApiRoute(_configuration, route, HttpMethod.Post);
            _configuration.ShouldMap(route).To<AccountController>(HttpMethod.Post, x => x.Register(null));
        }
    }
}