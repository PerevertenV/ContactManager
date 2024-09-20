using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CM.Data.Service.IService;

namespace CM.Data.Service
{
    public class UserService : IUserService
    {
        public string DecryptString(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decrypted = ProtectedData.Unprotect(encryptedBytes, null,
                DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }

        public string PasswordHashCoder(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] encrypted = ProtectedData.Protect(bytes, null,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }
    }
}
