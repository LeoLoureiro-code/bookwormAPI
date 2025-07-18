using bookwormAPI.EF.DataAccess.Context;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
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
            User user = await _context.Users.FindAsync(id);
            
            if (user == null) {

                throw new Exception("User not found");
             }

            return user;
        }


        //Create the service to hash password
        public Task<User> Createuser(User user)
        {
            
           
        }
    }
}
