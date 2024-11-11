using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesAPI.Infrastructure.Persistence.Contexts;
using SalesAPI.Infrastructure.SignalR;

namespace SalesAPI.Tests.Infrastructure
{
    public class SalesAPIApplication<T> : WebApplicationFactory<T> where T : class
    {
        private readonly string _testDbName;
      
        private readonly PostgresDbFixture _postgresDbFixture;

        public SalesAPIApplication(PostgresDbFixture postgresDbFixture)
        {
            _postgresDbFixture = postgresDbFixture;
            _testDbName = $"test_{Guid.NewGuid().ToString().Replace("-","")}"; // Unique database name for each test run
        }

        // This method configures the web host for testing
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration if it exists.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register DbContext with PostgreSQL and unique database name
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_postgresDbFixture.GetConnectionString(_testDbName));
                });

                // Apply migrations automatically for testing purposes
                services.AddTransient<IHostedService, DbMigrationsService>(); // Ensure migrations are applied

                // Register SignalR
                services.AddSignalR();
            });
           
        }

        // Cleanup after tests: drop the test database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _postgresDbFixture.CleanupTestDatabase(_testDbName); // Drop the database after test run
            }

            base.Dispose(disposing);
        }
    }

}
