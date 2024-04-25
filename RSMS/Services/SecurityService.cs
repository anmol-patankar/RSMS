﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RSMS.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RSMS.Services
{
    public static class SecurityService
    {
        private readonly static Aes _aesObject = Aes.Create();


        private static SymmetricSecurityKey securityKey;
        private static IConfiguration Config { get; set; }
        public static void SetKeyConfig(byte[] aesKey, IConfiguration config)
        {
            _aesObject.Key = aesKey;
            Config = config;
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
        }
        public static byte[] Decrypt(byte[] stringToDecrypt, byte[] salt)
        {
            using (var decryptor = _aesObject.CreateDecryptor())
            {
                _aesObject.IV = salt;
                return (decryptor.TransformFinalBlock(stringToDecrypt, 0, stringToDecrypt.Length));
            }
        }
        private static byte[] Encrypt(string stringToEncrypt)
        {
            using (var encryptor = _aesObject.CreateEncryptor())
            {
                byte[] stringToEncryptAsArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                return encryptor.TransformFinalBlock(stringToEncryptAsArray, 0, stringToEncryptAsArray.Length);
            }
        }
        public static byte[] HashStringWithSalt(string password, byte[] salt)
        {
            _aesObject.IV = salt;
            return Encrypt(password);
        }
        public static byte[] GetHashedAndSaltedString(string password, out byte[] aesIV)
        {
            _aesObject.GenerateIV();
            aesIV = _aesObject.IV;
            return Encrypt(password);
        }
        public static string GenerateEncryptedToken(UserLoginModel login, out byte[] currentUserSalt)
        {
            currentUserSalt = DatabaseService.GetUser(login.Username).Salt;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userRoles = DatabaseService.GetRolesOfUser(login.Username);
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, login.Username) };
            foreach (var roles in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, roles));
            var token = new JwtSecurityToken(Config["Jwt:Issuer"],
                Config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
            //return Convert.ToHexString(HashStringWithSalt(new JwtSecurityTokenHandler().WriteToken(token), currentUserSalt));
        }
    }
}
