using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using UriShortener.Data.Model;
using UriShortener.Data.Repository;
using UriShortener.Options;
using UriShortener.Options.Auth;
using UriShortener.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var constr = configuration.GetConnectionString("Default");
var jwtOpts = configuration.GetSection("Jwt").Get<JwtOptions>();

if (jwtOpts is null)
    throw new Exception("Configuration Missing Crucial Data");

// Options
builder.Services.AddOptions<JwtOptions>()
    .Bind(configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<EmailVerificationTokenOptions>()
    .Bind(configuration.GetSection("EmailVerificationToken"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<PasswordResetTokenOptions>()
    .Bind(configuration.GetSection("PasswordResetToken"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<EmailOptions>()
    .Bind(configuration.GetSection("Email"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<ShortUriOptions>()
    .Bind(configuration.GetSection("ShortUri"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUriService, UriService>();
builder.Services.AddScoped<IEMailService, EMailService>();

builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContextPool<AppDbContext>(opts => {
    opts.UseSqlServer(constr);
    // opts.EnableSensitiveDataLogging();
    // opts.LogTo(Console.WriteLine, LogLevel.Trace);
});
builder.Services.AddAuthentication()
    .AddJwtBearer(BearerTokenDefaults.AuthenticationScheme, opts => {
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOpts.Issuer,
            ValidAudience = jwtOpts.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.SigningKey))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();