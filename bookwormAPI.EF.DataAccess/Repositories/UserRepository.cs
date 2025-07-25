using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using bookwormAPI.EF.DataAccess.Services;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {

        private readonly BookwormContext _context; 

        public UserRepository(BookwormContext context) {
            _context = context; 
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            
            if (user == null) {

                throw new Exception("User not found");
             }

            return user;
        }

        public async Task<User> GetUserByName(string username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) 
            {
                throw new Exception("User not found.");
            }

            //Use PaswordService to check the password too and then return user

        }

        public async Task<User> CreateUser(UserDTO user)
        {
            var passwordService = new PasswordService();
            string hashed = passwordService.HashPassword(user.Password);
           
            user.Password = hashed;

            var userEntity = new User
            {
                UserName = user.Email,
                UserPasswordHash = hashed,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                RevokedAt = DateTime.UtcNow.AddMinutes(15),
                IsActive = true,
            };


            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        //Check this in case of a mistake
        public async Task<User> UpdateUser(int id, string name, string passwordHashed )
        {
            User? existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null) 
            {
                throw new Exception("User not found");        
            }

            var passwordService = new PasswordService();
            string hashed = passwordService.HashPassword(passwordHashed);

            existingUser.UserName = name;
            existingUser.UserPasswordHash = hashed;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteUser (int id)
        {
            User? existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
        }

    }
}
