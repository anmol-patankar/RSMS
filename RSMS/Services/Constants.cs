namespace RSMS.Services
{
    public static class Constants
    {
        public static string JwtKeyString { get; } = "Jwt:Key";
        public static string JwtKeyIssuer { get; } = "Jwt:Issuer";
        public static string JwtKeyAudience { get; } = "Jwt:Audience";
        public static string DbConnectionString { get; } = "dbcs";
        public static string AppSettingsFile { get; } = "appsettings.json";
        public static string AesKeyString { get; } = "AesEncryption:Key";
    }
}
