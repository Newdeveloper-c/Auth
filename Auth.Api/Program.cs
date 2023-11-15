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
