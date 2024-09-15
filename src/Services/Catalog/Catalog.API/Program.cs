using Catalog.API;
using Catalog.API.Mapster;
using GameVault.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// set config builder
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


// set connection string
var connectionString = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Database"))
    ? config.GetConnectionString("Database")
    : Environment.GetEnvironmentVariable("Database");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));


// Add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.RegisterMapsterConfigurations();
builder.Services.AddCommonServices();

builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseMySql(connectionString, serverVersion), ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTPS request pipeling

app.MapCarter();

app.Run();
