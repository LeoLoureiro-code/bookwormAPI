using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(int id);

        Task<User> CreateUser(UserDTO user);

        Task<User> UpdateUser(int id, string name, string passwordhashed);

        Task DeleteUser(int id);

    }
}
