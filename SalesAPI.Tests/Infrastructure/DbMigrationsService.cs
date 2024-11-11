using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesAPI.Infrastructure.Persistence.Contexts;

namespace SalesAPI.Tests.Infrastructure
{
    public class DbMigrationsService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbMigrationsService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Apply migrations to the database
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // No specific logic needed for stopping the migrations service
            return Task.CompletedTask;
        }
    }

}
