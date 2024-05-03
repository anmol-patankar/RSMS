﻿using Microsoft.IdentityModel.Tokens;
using RSMS.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RSMS.Services
{
    public static class SecurityService
    {
        private static readonly Aes _aesObject = Aes.Create();

        private static SymmetricSecurityKey securityKey;
        private static IConfiguration Config { get; set; }

        public static void SetKeyConfig(string aesKey, IConfiguration config)
        {
            _aesObject.Key = Encoding.UTF8.GetBytes(aesKey);
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

        public static byte[] HashStringWithSalt(string stringToEncrypt, byte[] salt)
        {
            _aesObject.IV = salt;
            return Encrypt(stringToEncrypt);
        }

        public static byte[] GetHashedAndSaltedString(string stringToEncrypt, out byte[] aesIV)
        {
            _aesObject.GenerateIV();
            aesIV = _aesObject.IV;
            return Encrypt(stringToEncrypt);
        }

        public static string GenerateEncryptedToken(UserLoginModel login)
        {

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userRoles = DatabaseService.GetRolesOfUser(login.Username);
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, login.Username), new(ClaimTypes.Name, login.Username) };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)));
            var token = new JwtSecurityToken(
                issuer: Config["Jwt:Issuer"],
                audience: Config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}