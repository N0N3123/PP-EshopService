using System;
using User.Domain.Exceptions.Login;
using Xunit;

namespace User.Domain.Tests
{
    public class InvalidCredentialsExceptionTests
    {
        [Fact]
        public void InvalidCredentialsException_HasDefaultMessage()
        {
            // Arrange & Act
            var ex = new InvalidCredentialsException();

            // Assert
            Assert.NotNull(ex);
            Assert.Contains("Invalid credentials", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}