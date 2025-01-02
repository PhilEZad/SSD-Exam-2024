using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security;
using RegisterApplication = Application.DependencyResolver.DependencyInjectionResolver;
using RegisterInfrastructure = Infrastructure.DependencyResolver.DependencyInjectionResolver;
using RegisterSecurity = Security.DependencyResolver.DependencyInjectionResolver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Registers
builder.Services.AddControllers();
RegisterApplication.RegisterApplicationLayer(builder.Services);
RegisterInfrastructure.RegisterInfrastructure(builder.Services);
RegisterSecurity.RegisterSecurity(builder.Services);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// Load Vault secrets
var vault = new VaultService();
var jwtSecret = await vault.GetSecretAsync("/data/jwt", "key");
var hashSecret = await vault.GetSecretAsync("/data/hash", "key");
var databaseConnection = await vault.GetSecretAsync("/data/database", "key");

// Seal after keys have been retrieved
await vault.SealVaultAsync();

// Use keys from loaded vault
builder.Services.PostConfigure<JwtOptions>(options =>
{
    options.Key = jwtSecret;
});

builder.Services.PostConfigure<HashOptions>(options =>
{
    options.Key = hashSecret;
});

// Database Connection
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(databaseConnection));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // TODO: Remember to change to true before delivering
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = "",
            ValidAudience = "",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), // Use the Vault-provided key
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token successfully validated.");
                return Task.CompletedTask;
            }
        };
    });

// Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", policy =>
    {
        policy.Window = TimeSpan.FromSeconds(10);
        policy.PermitLimit = 5;
        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        policy.QueueLimit = 2;
    });
});

// TODO: add and configure cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost",
        corsBuilder => corsBuilder
            // Check if host is correct
            .WithOrigins("https://localhost:8000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyOrigin()
    );
});


// {
//     var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//     var certPath = Path.Combine(path, "aspnetapp.pfx");
//     builder.WebHost.ConfigureKestrel(options =>
//     {
//         options.ListenLocalhost(5000, listenOptions =>
//         {
//             listenOptions.UseHttps(certPath, "mysslpassword");
//         });
//     });
// }





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseRouting();

app.UseCors("AllowAngularLocalhost");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();