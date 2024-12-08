using FluentValidation;
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
RegisterSecurity.RegisterSecurity(builder.Services);
RegisterApplication.RegisterApplicationLayer(builder.Services);
RegisterInfrastructure.RegisterInfrastructure(builder.Services);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

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