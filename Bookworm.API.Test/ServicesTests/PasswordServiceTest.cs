using bookwormAPI.EF.DataAccess.Services;
using Xunit;

namespace bookwormAPI.Tests.Services
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedValue()
        {
            // Arrange
            var password = "MySecret123!";

            // Act
            var hashedPassword = _passwordService.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrEmpty(hashedPassword));
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
        {
            // Arrange
            var password = "MySecret123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var isValid = _passwordService.VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
        {
            // Arrange
            var password = "MySecret123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var isValid = _passwordService.VerifyPassword(hashedPassword, "WrongPassword");

            // Assert
            Assert.False(isValid);
        }
    }
}
