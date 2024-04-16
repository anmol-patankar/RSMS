using Domain.Models;
using RSMS.ViewModels;
using System.Text;
using System.Security.Cryptography;
namespace RSMS.Services
{
    public static class RegistrationService
    {
        public static void RegisterUser(UserRegistrationModel user, byte[] aesKey, out UserInfo userInfo)
        {
            byte[] aesIV;

            byte[] HashPassword(string password, byte[] aesKey)
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
            var passwordHashed = HashPassword(user.Password, aesKey);
            userInfo = new UserInfo()
            {
                Username = user.Username,
                Dob = user.Dob,
                Email = user.Email,
                Salt = aesIV,
                PasswordHashed = passwordHashed,
                Phone = user.Phone,
                RegistrationDate = DateTime.Now,
            };
        }
    }
}
