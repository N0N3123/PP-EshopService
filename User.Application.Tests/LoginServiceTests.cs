using System;
using System.Collections.Generic;
using User.Application.Services;
using User.Domain.Exceptions.Login;
using Xunit;

namespace User.Application.Tests
{
    // Prosta implementacja IJwtTokenService do testów
    public class FakeJwtTokenService : IJwtTokenService
    {
        public string GenerateToken(int userId, List<string> roles)
        {
            return $"token-{userId}-{string.Join(",", roles)}";
        }
    }

    public class LoginServiceTests
    {
        [Fact]
        public void Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var jwtService = new FakeJwtTokenService();
            var service = new LoginService(jwtService);

            // Act
            var token = service.Login("admin", "password");

            // Assert
            Assert.StartsWith("token-123-", token);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ThrowsException()
        {
            // Arrange
            var jwtService = new FakeJwtTokenService();
            var service = new LoginService(jwtService);

            // Act & Assert
            Assert.Throws<InvalidCredentialsException>(() => service.Login("user", "wrong"));
        }
    }
}