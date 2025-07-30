using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using bookwormAPI.EF.DataAccess.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Services
{
    public class AuthService: IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService, IConfiguration config)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _config = config;
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByNameAndPasswordAsync(username, password);
            if (user == null)
                throw new Exception("User not found.");

            if (!_passwordService.VerifyPassword(user.UserPasswordHash, password))
                throw new Exception("Invalid password.");

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            
            user.RefreshToken = refreshToken;
            user.ExpiresAt = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(user.UserId, user.UserName, user.UserPasswordHash);

            return (accessToken, refreshToken);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        }),
                Expires = DateTime.UtcNow.AddMinutes(15), 
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task RevokeRefreshTokenAsync(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new Exception("User not found.");

            user.RefreshToken = null;
            user.ExpiresAt = null;

            await _userRepository.UpdateUser(user.UserId, user.UserName, user.UserPasswordHash);
        }
    }
}
