using Domain.Models;
using Microsoft.Identity.Client;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Text;

namespace RSMS.Services
{
    public static class EncryptionService
    {
        private readonly static Aes _aesObject = Aes.Create();
        public static void SetKey(byte[] aesKey)
        { _aesObject.Key = aesKey; }
        private static byte[] PasswordEncryptor(string password)
        {

            using (var encryptor = _aesObject.CreateEncryptor())
            {
                byte[] passwordAsByteArray = Encoding.UTF8.GetBytes(password);
                return encryptor.TransformFinalBlock(passwordAsByteArray, 0, passwordAsByteArray.Length);
            }
        }
        public static byte[] VerifyPassword(string password, byte[] salt)
        {
            _aesObject.IV = salt;
            return PasswordEncryptor(password);
        }
        public static byte[] HashPassword(string password, out byte[] aesIV)
        {
            _aesObject.GenerateIV();
            aesIV = _aesObject.IV;
            return PasswordEncryptor(password);
        }
    }
}
