using System.Security.Cryptography;
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
        public static string GenerateToken(UserLoginModel login)
        {
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
        }
    }
}
