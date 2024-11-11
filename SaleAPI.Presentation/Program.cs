using Microsoft.AspNetCore.Mvc;
using SaleAPI.Presentation.Extensions;
using SalesAPI.Infrastructure;
using SalesAPI.Application;
using SalesAPI.Infrastructure.Persistence.Data;
using Serilog;
using Microsoft.OpenApi.Models;
using SalesAPI.Presentation.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddInfrastructure(config, Log.Logger);
builder.Services.AddApplication();

//builder.Services.AddControllers();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomAuthorizeAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

   
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales API",
        Version = "v1",
        Description = "This is a prototype"
      
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                    }
                });

});


builder.Services.AddApiVersioning(options =>
{
    // Set up versioning conventions
    options.ReportApiVersions = true;  
    options.AssumeDefaultVersionWhenUnspecified = true;  
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync(scope.ServiceProvider, "admin@admin.com", "Admin@12345"); // Seed data for Products and Sales Orders
}

// Configure the HTTP request pipeline.

app.UseSerilogRequestLogging();
app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

