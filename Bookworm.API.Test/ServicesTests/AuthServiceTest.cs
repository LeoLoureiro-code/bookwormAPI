using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using bookwormAPI.EF.DataAccess.Services;
using bookwormAPI.EF.DataAccess.Services.Interfaces;
using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkey1234567890");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("audience");

        _authService = new AuthService(
            _userRepoMock.Object,
            _passwordServiceMock.Object,
            _configMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsTokens()
    {
        // Arrange
        var fakeUser = new User
        {
            UserId = 1,
            UserName = "test@example.com",
            UserPasswordHash = "hashedpassword"
        };

        var configMock = new Mock<IConfiguration>();
        var testKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        configMock.Setup(c => c["Jwt:Key"]).Returns(testKey);
        configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repo => repo.GetUserByName("test@example.com"))
                    .ReturnsAsync(fakeUser);

        var passwordServiceMock = new Mock<IPasswordService>();
        passwordServiceMock.Setup(p => p.VerifyPassword("hashedpassword", "123456"))
                           .Returns(true);

        userRepoMock.Setup(repo => repo.UpdateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(fakeUser);

        var authService = new AuthService(userRepoMock.Object, passwordServiceMock.Object, configMock.Object);

        // Act
        var (accessToken, refreshToken) = await authService.LoginAsync("test@example.com", "123456");

        // Assert
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.False(string.IsNullOrEmpty(refreshToken));
    }


    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsException()
    {
        var fakeUser = new User { UserName = "test@example.com", UserPasswordHash = "hashed" };

        _userRepoMock.Setup(r => r.GetUserByName("test@example.com")).ReturnsAsync(fakeUser);
        _passwordServiceMock.Setup(p => p.VerifyPassword("hashed", "wrongpass")).Returns(false);

        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync("test@example.com", "wrongpass"));
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsException()
    {
        _userRepoMock.Setup(r => r.GetUserByName("nouser@example.com")).ThrowsAsync(new Exception("User not found."));

        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync("nouser@example.com", "123456"));
    }
}