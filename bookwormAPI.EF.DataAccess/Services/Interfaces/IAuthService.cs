using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(string username, string password);

    }
}
