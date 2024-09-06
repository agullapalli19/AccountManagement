using AccountManagementAPI;
using AccountManagementAPI.Model;
using AccountManagementAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Generate RSA keys at startup
RSAKeyPair.GenerateKeys(out string publicKey, out string privateKey);
// Register the keys in the DI container
builder.Services.AddSingleton(new RSAKeys(publicKey, privateKey));

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("AccountDB");
builder.Services.AddDbContext<AccountDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddScoped<IAccountsRepo, AccountsRepo>();

builder.Services.AddLogging();


var authKey = builder.Configuration.GetValue<String>("JWTSetting:securityKey");
var _jwtSetting = builder.Configuration.GetSection("JWTSetting");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "http://localhost:5224")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 //app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

// RSAKeys class definition to hold the keys
public class RSAKeys
{
    public string PublicKey { get; }
    public string PrivateKey { get; }

    public RSAKeys(string publicKey, string privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
}

// RSAKeyPair class for generating keys
public static class RSAKeyPair
{
    public static void GenerateKeys(out string publicKey, out string privateKey)
    {
        using (var rsa = RSA.Create())
        {
            publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        }
    }
}
