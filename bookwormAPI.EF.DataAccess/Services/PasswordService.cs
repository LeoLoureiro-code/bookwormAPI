using bookwormAPI.EF.DataAccess.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.Services
{
    public class PasswordService: IPasswordService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }



        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }
    }
}
