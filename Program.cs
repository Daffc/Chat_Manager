using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetEscapades.Extensions.Logging.RollingFile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Chat Manager API",
            Description = "API for Chat Manager features."
        });
    });

// Domain
builder.Services
    .AddScoped<IPasswordHasher, BcryptPasswordHasher>()
    .AddScoped<UserFactory>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();

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