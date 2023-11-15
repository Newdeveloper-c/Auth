using Auth.Domain.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UnathorizedUser", policy => policy.RequireRole("Unauthorized"));
    options.AddPolicy("AthorizedUser", policy => policy.RequireRole("Authorized"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Ceo", policy => policy.RequireRole("Ceo"));
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
builder.Services.AddRouting(r =>
{
    r.LowercaseUrls = true;
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
