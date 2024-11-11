using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesAPI.Application.Interfaces.Auth;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;
using SalesAPI.Infrastructure.Identity;
using SalesAPI.Infrastructure.Persistence.Contexts;
using SalesAPI.Infrastructure.Persistence.Data;
using SalesAPI.Infrastructure.Persistence.Repositories;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace SalesAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                

            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            // Register the DbSeeder
            services.AddScoped<DbSeeder>();

            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            // Add Identity services

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // This automatically checks the token expiration
            ClockSkew = TimeSpan.Zero,  // Optional: Set clock skew (default is 5 minutes)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"JWT Token received: {token}");
                logger.Error("JWT Token received: {token}", token);
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    logger.Error("Authentication failed: {Message}", context.Exception.Message);
                    return context.Response.WriteAsync("{\"error\":\"Token has expired\"}");
                }

                if (context.Exception is SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"error\":\"Invalid token\"}");
                }

                return Task.CompletedTask;
            }
        };
    });



        }

    }
}
