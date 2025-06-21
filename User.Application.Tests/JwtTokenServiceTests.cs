using Microsoft.Extensions.Options;
using System.Collections.Generic;
using User.Application.Services;
using User.Domain.Models.JWT;
using Xunit;

namespace User.Application.Tests
{
    public class JwtTokenServiceTests
    {
        [Fact]
        public void GenerateToken_ReturnsTokenString()
        {
            // Arrange
            var settings = Options.Create(new JwtSettings
            {
                Key = "supersecretkeysupersecretkey", // minimum 16 chars for HMAC
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiresInMinutes = 60
            });
            var service = new JwtTokenService(settings);

            // Act
            var token = service.GenerateToken(1, new List<string> { "User" });

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }
    }
}