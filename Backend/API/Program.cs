using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Security;
using RegisterApplication = Application.DependencyResolver.DependencyInjectionResolver;
using RegisterInfrastructure = Infrastructure.DependencyResolver.DependencyInjectionResolver;
using RegisterSecurity = Security.DependencyResolver.DependencyInjectionResolver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registers
builder.Services.AddControllers();
RegisterApplication.RegisterApplicationLayer(builder.Services);
RegisterInfrastructure.RegisterInfrastructure(builder.Services);
RegisterSecurity.RegisterSecurity(builder.Services);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Load Vault secrets
var vault = new VaultService();
var jwtSecret = await vault.GetSecretAsync("/data/jwt", "key");
var hashSecret = await vault.GetSecretAsync("/data/hash", "key");
var databaseConnection = await vault.GetSecretAsync("/data/database", "key");

await vault.SealVaultAsync(); // Seal after keys have been retrieved

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
    options.UseSqlServer(builder.Configuration.GetConnectionString(databaseConnection)));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();

app.MapControllers();

app.Run();