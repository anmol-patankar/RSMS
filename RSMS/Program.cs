using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RSMS.Services;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtIssuer = builder.Configuration.GetSection(Constants.JwtKeyIssuer).Get<string>();

var jwtKey = builder.Configuration.GetSection(Constants.JwtKeyString).Get<string>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RsmsTestContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(Constants.DbConnectionString)));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Constants.JwtKeyString])),

        ValidIssuer = builder.Configuration[Constants.JwtKeyIssuer],

        ValidAudience = builder.Configuration[Constants.JwtKeyAudience],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
builder.Configuration.AddJsonFile(Constants.AppSettingsFile, optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Please log in to access this resource.");
        }
        else
        {
            string loginUrl = $"/User/Login";
            context.Response.Redirect(loginUrl);
        }
    }
});

app.Use(async (context, next) =>
{
    var JWTokenCookie = context.Request.Cookies["JWTToken"];
    if (!string.IsNullOrEmpty(JWTokenCookie))
    {
        context.Request.Headers.Append("Authorization", "Bearer " + JWTokenCookie);
    }
    await next();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();