using bookwormAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password);


        Task RevokeAndRefreshTokenAsync(string email, string password);

    }
}
