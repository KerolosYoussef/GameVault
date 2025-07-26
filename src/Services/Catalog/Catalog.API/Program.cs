using Catalog.API;
using Catalog.API.Helpers;
using Catalog.API.Mapster;
using GameVault.Common;
using GameVault.Common.Behaviors;
using GameVault.Common.Exceptions.Handlers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

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
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.RegisterMapsterConfigurations();
builder.Services.AddCommonServices();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseMySql(connectionString, serverVersion), ServiceLifetime.Transient);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddMySql(connectionString!);

var app = builder.Build();

app.Services.MigrateData();

// Configure the HTTPS request pipeling

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
