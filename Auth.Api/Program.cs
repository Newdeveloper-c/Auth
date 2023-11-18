using Auth.Application.Interfaces;
using Auth.Application.Options;
using Auth.Domain.Context;
using Auth.Infrastructure.Managers;
using Auth.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("AuthDb");

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(conString);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(j =>
    {
        j.TokenValidationParameters = new TokenValidationParameters
        {
            // Tekshirish qismi
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,

            // Ruxsat berilgan qiymatlar ro'yxati 
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthManager, AuthManager>()
    .AddScoped<ITokenManager, TokenManager>()
    .AddScoped<IPasswordManager, PasswordManager>()
    .AddScoped<GlobalExceptionHandlingMiddlware>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("Authorization");
});


builder.Services.AddControllers();
builder.Services.AddRouting(r =>
{
    r.LowercaseUrls = true;
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddlware>();

app.UseHeaderPropagation();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
