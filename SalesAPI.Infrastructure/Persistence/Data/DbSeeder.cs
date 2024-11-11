using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SalesAPI.Domain.Entities;
using SalesAPI.Infrastructure.Persistence.Contexts;

namespace SalesAPI.Infrastructure.Persistence.Data
{
    public class DbSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        ILogger<DbSeeder> logger, ApplicationDbContext context)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<DbSeeder> _logger = logger;

        public async Task SeedAsync(IServiceProvider serviceProvider, string adminEmail, string adminPassword)
        {
            try
            {
                // Ensure the database is created
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.EnsureCreatedAsync();

                // Seed roles
                await SeedRolesAsync();

                // Seed the default admin user
                await SeedAdminUserAsync(adminEmail, adminPassword);

                await SeedProductsAsync();
                await SeedSalesOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task SeedAdminUserAsync(string adminEmail, string adminPassword)
        {
            var user = await _userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                user = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    _logger.LogInformation("Admin user seeded successfully.");
                }
                else
                {
                    _logger.LogError("Failed to seed admin user: {0}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists.");
            }
        }

        private async Task SeedProductsAsync()
        {
            // Check if products already exist
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    Product.Create("Laptop",  1500.00m),
                    Product.Create("Smartphone",  300.00m),
                    Product.Create("Headphones",  100.00m),
                    Product.Create("Smartwatch",  250.00m)
                };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Products seeded successfully.");
            }
            else
            {
                _logger.LogInformation("Products already exist in the database.");
            }
        }

        private async Task SeedSalesOrdersAsync()
        {
            // Check if sales orders already exist
            if (!_context.SalesOrders.Any())
            {
                var user = _context.Users.FirstOrDefault();
                var laptop = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Laptop");
                var smartphone = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Smartphone");

                if (laptop == null || smartphone == null || user == null)
                {
                    _logger.LogError("Required products for seeding sales orders are missing.");
                    return;
                }

                var salesOrders = new List<SalesOrder>
                {
                  SalesOrder.Create(laptop.Id, Guid.Parse(user.Id), 12, laptop.Price),
                  SalesOrder.Create(smartphone.Id, Guid.Parse(user.Id), 25, smartphone.Price),
                  SalesOrder.Create(laptop.Id, Guid.Parse(user.Id), 12, laptop.Price),
                };

                await _context.SalesOrders.AddRangeAsync(salesOrders);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Sales orders seeded successfully.");
            }
            else
            {
                _logger.LogInformation("Sales orders already exist in the database.");
            }
        }
    }
}

