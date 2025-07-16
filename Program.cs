using Domain.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
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

