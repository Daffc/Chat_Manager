using Domain.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using NetEscapades.Extensions.Logging.RollingFile; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(); 

// Domain
builder.Services
    .AddScoped<IPasswordHasher, BcryptPasswordHasher>()
    .AddScoped<UserFactory>();

// Infrastructure
builder.Services
    .AddDbContext<AppDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IUserRepository, UserRepository>();

// Application
builder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly));

// WebApi
builder.Services.AddControllers();


// Logging in files only on production environment.
if (builder.Environment.IsProduction())
{
    const string logDirectory = "Logs";
    Directory.CreateDirectory(logDirectory);

    builder.Logging.AddFile(options =>
    {
        options.Periodicity = PeriodicityOptions.Daily;
        options.LogDirectory = logDirectory;
        options.FileName = "chat_manager-";
        options.Extension = ".log";
        options.RetainedFileCountLimit = 7;
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();