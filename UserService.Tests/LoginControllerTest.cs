using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Exceptions.Login;
using User.Domain.Models.Requests;
using UserService.Controllers;
using Xunit;

namespace UserService.Tests
{
    // Simple fake implementation for testing
    public class FakeLoginService : ILoginService
    {
        public string Login(string username, string password)
        {
            if (username == "admin" && password == "password")
                return "test-token";
            throw new InvalidCredentialsException();
        }
    }

    public class LoginControllerTests
    {
        [Fact]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var controller = new LoginController(new FakeLoginService());
            var request = new LoginRequest { Username = "admin", Password = "password" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.Contains("token", okResult.Value.ToString());
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new LoginController(new FakeLoginService());
            var request = new LoginRequest { Username = "user", Password = "wrong" };

            // Act
            var result = controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}