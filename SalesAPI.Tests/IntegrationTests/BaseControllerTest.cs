using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.Auth;
using SalesAPI.Application.Features.Auth.Commands;
using SalesAPI.Infrastructure.Persistence.Contexts;
using SalesAPI.Tests.Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SalesAPI.Tests.IntegrationTests
{
    public abstract class BaseControllerTest<TController> : IClassFixture<DatabaseFixture>, IDisposable where TController : ControllerBase
    {
        protected readonly SalesAPIApplication<TController> _application;
        protected readonly HttpClient _client;
        protected ApplicationDbContext _dbContext;
        protected readonly PostgresDbFixture _postgresDbFixture;

        // Constructor now accepts the DatabaseFixture that provides PostgresDbFixture
        protected BaseControllerTest(DatabaseFixture databaseFixture)
        {
            _postgresDbFixture = databaseFixture.PostgresDbFixture;  // Access the PostgresDbFixture

            // Initialize SalesAPIApplication with the PostgresDbFixture
            _application = new SalesAPIApplication<TController>(_postgresDbFixture);

            // Create an HTTP client to interact with the application
            _client = _application.CreateClient();

            // Access the DbContext
            var scope = _application.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
        protected virtual async Task AuthenticateAsync()
        {
            // Arrange
            var payload = new LoginCommand { Email = "admin@admin.com", Password = "Admin@12345" };
            var payloadJson = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client
                .PostAsync($"/api/v1/Auth/login", payloadJson);

            var result = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseData = JsonConvert.DeserializeObject<Result<TokenDto>>(result);
            responseData.Should().NotBeNull();
            responseData.Succeeded.Should().BeTrue();
            responseData.Data.Should().NotBeNull();
            responseData.Data.RefreshToken.Should().NotBeNull();
            responseData.Data.AccessToken.Should().NotBeNull();
           
            var token = responseData.Data.AccessToken;

            // Add the Authorization header for all subsequent requests
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        

        }
        public virtual void Dispose()
        {
            // Dispose of resources
            _client.Dispose();
            _application.Dispose();
        }
    }

}
