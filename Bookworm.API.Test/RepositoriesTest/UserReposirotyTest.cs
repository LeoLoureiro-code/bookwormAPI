using System;
using System.Linq;
using System.Threading.Tasks;
using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories;
using bookwormAPI.EF.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class UserRepositoryTests
{
    private BookwormContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BookwormContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new BookwormContext(options);
    }

    private Mock<IPasswordService> GetPasswordServiceMock()
    {
        var mock = new Mock<IPasswordService>();
        mock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        return mock;
    }

    [Fact]
    public async Task CreateUser_ShouldAddUser_WhenEmailIsUnique()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();

        var repo = new UserRepository(context, passwordService.Object);

        var userDto = new UserDTO { Email = "test@example.com", Password = "123456" };

        var createdUser = await repo.CreateUser(userDto);

        Assert.NotNull(createdUser);
        Assert.Equal("test@example.com", createdUser.UserName);
        Assert.Equal("hashed_password", createdUser.UserPasswordHash);
        Assert.Single(context.Users);
    }

    [Fact]
    public async Task CreateUser_ShouldThrow_WhenEmailExists()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        context.Users.Add(new User { UserName = "test@example.com", UserPasswordHash = "hash" });
        await context.SaveChangesAsync();

        var userDto = new UserDTO { Email = "test@example.com", Password = "123456" };

        await Assert.ThrowsAsync<Exception>(() => repo.CreateUser(userDto));
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenExists()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        var user = new User { UserName = "test@example.com", UserPasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await repo.GetUserById(user.UserId);

        Assert.NotNull(result);
        Assert.Equal(user.UserName, result.UserName);
    }

    [Fact]
    public async Task GetUserById_ShouldThrow_WhenNotFound()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        await Assert.ThrowsAsync<Exception>(() => repo.GetUserById(999));
    }

    [Fact]
    public async Task GetUserByName_ShouldReturnUser_WhenExists()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        var user = new User { UserName = "test@example.com", UserPasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await repo.GetUserByName("test@example.com");

        Assert.NotNull(result);
        Assert.Equal(user.UserName, result.UserName);
    }

    [Fact]
    public async Task GetUserByName_ShouldThrow_WhenNotFound()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        await Assert.ThrowsAsync<Exception>(() => repo.GetUserByName("notfound@example.com"));
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateData_WhenExists()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        var user = new User { UserName = "old@example.com", UserPasswordHash = "oldhash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var updated = await repo.UpdateUser(user.UserId, "new@example.com", "newhash");

        Assert.Equal("new@example.com", updated.UserName);
        Assert.Equal("newhash", updated.UserPasswordHash);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrow_WhenNotFound()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        await Assert.ThrowsAsync<Exception>(() => repo.UpdateUser(999, "name", "hash"));
    }

    [Fact]
    public async Task DeleteUser_ShouldRemove_WhenExists()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        var user = new User { UserName = "test@example.com", UserPasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        await repo.DeleteUser(user.UserId);

        Assert.Empty(context.Users);
    }

    [Fact]
    public async Task DeleteUser_ShouldThrow_WhenNotFound()
    {
        var context = GetDbContext();
        var passwordService = GetPasswordServiceMock();
        var repo = new UserRepository(context, passwordService.Object);

        await Assert.ThrowsAsync<Exception>(() => repo.DeleteUser(999));
    }
}
