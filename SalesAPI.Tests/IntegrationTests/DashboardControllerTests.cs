using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using SaleAPI.Presentation.Controllers;
using SalesAPI.Application.Commons;
using SalesAPI.Tests.Infrastructure;
using SalesAPI.Application.DTOs.SalesOrders;
using SalesAPI.Tests.Helpers;

namespace SalesAPI.Tests.IntegrationTests
{
    public class DashboardControllerTests(DatabaseFixture dbFixture) : BaseControllerTest<DashboardController>(dbFixture)
    {
      
     

        [Fact]
        public async Task GetHighestSoldProduct_ReturnsOkResult_WithValidData()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await _client.GetAsync($"{UrlHelper.DashboardPath}/overview");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);  // Check for 200 OK status

           
            var result = await response.Content.ReadFromJsonAsync<Result<GetHighestSoldProductDto>>();
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.HighestPrice.Should().BeGreaterThanOrEqualTo(0);
            result.Data.HighestSoldProductQuantity.Should().BeGreaterThanOrEqualTo(0);
            result.Data.HighestSoldProduct.Should().NotBeEmpty();
            result.Data.HighestPricedProduct.Should().NotBeEmpty();
          
        }

        // Test for Unauthorized access (if no token is provided)
        [Fact]
        public async Task GetHighestSoldProduct_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync($"{UrlHelper.DashboardPath}/overview");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);  
        }
    }
}
