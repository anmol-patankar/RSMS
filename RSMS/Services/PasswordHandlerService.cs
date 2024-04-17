using System.Text;
using System.Security.Cryptography;
namespace RSMS.Services
{
    public static class PasswordHandlerService
    {
        public static byte[] VerifyPassword(string password, byte[] aesKey, byte[] salt)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = salt;
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] saltedPassword = Encoding.UTF8.GetBytes(password);
                    return encryptor.TransformFinalBlock(saltedPassword, 0, saltedPassword.Length);
                }
            }
        }
        public static byte[] HashPassword(string password, byte[] aesKey, out byte[] aesIV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = aesKey;
                aesIV = aes.IV;
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] saltedPassword = Encoding.UTF8.GetBytes(password);
                    return encryptor.TransformFinalBlock(saltedPassword, 0, saltedPassword.Length);
                }
            }
        }

    }
}
