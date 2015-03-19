using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Database.DatabaseModels;
using Database.Repositories;
using Effort;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.TestUtilities;
using WebApi.Controllers;

namespace Tests.WebApiTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private MoviepickerContext _context;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            _context = new MoviepickerContext(DbConnectionFactory.CreateTransient());
            _userRepository = new UserRepository(_context);
            _accountController = new AccountController(_userRepository) {Configuration = new HttpConfiguration()};
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithoutUsername_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            user.Username = null;

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithoutPassword_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            user.Password = null;

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithoutConfirmPassword_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            user.ConfirmPassword = null;

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithTooShortPassword_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            user.Password = "123";
            user.ConfirmPassword = "123";

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithDifferingPasswordAndConfirmPassword_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            user.Password = "123";
            user.ConfirmPassword = "123456";

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithTooLongPassword_ReturnsInvalidModelState()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();
            var password = new string(Enumerable.Repeat("a", 105).SelectMany(x => x).ToArray());
            user.Password = password;
            user.ConfirmPassword = password;

            // Act
            _accountController.Validate(user);
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<InvalidModelStateResult>().Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("Unit_CONTROLLER")]
        public async Task RegisterUser_WithValidModel_CreatesUser()
        {
            // Arrange
            var user = TestDataProvider.GetUserModel();

            // Act
            var response = await _accountController.RegisterAsync(user);

            // Assert
            response.As<OkResult>().Should().NotBeNull();
            _context.Users.Should().HaveCount(1);
        }
    }
}