using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetEscapades.Extensions.Logging.RollingFile;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Domain
builder.Services
    .AddScoped<IPasswordHasher, BcryptPasswordHasher>()
    .AddScoped<IJwtService, JwtService>()
    .AddScoped<UserFactory>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();

// Infrastructure
builder.Services
    .AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IUserRepository, UserRepository>();

// Application
builder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly))
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserHandler).Assembly));

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

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT:Key not set in configuration.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT:Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT:Audience not set in configuration.");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();