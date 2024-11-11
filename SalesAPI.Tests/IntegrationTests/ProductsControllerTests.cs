using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SalesAPI.Application.Features.Products.Commands;
using SalesAPI.Application.Features.Products.Queries;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using SalesAPI.Presentation.Controllers;
using SalesAPI.Tests.Infrastructure;
using FluentAssertions;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.Auth;
using SalesAPI.Tests.Helpers;
using SalesAPI.Application.DTOs.Products;
using SaleAPI.Presentation.Controllers;

namespace SalesAPI.Tests.IntegrationTests
{
   public class ProductsControllerTests(DatabaseFixture dbFixture) : BaseControllerTest<ProductsController>(dbFixture)
    {
       
        [Fact]
        public async Task CreateProduct_ShouldReturnsOkAtActionResult()
        {
            // Arrange
            await AuthenticateAsync();  // Ensure authentication before making API calls

            var createProductCommand = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m,
            };

            // Act
            var response = await _client.PostAsJsonAsync(UrlHelper.ProductPath, createProductCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseData = JsonConvert.DeserializeObject<Result<Guid>>(result);
            responseData.Should().NotBeNull();
            responseData.Succeeded.Should().BeTrue();
            responseData.Data.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithCorrectData()
        {
            // Arrange
            await AuthenticateAsync();  

          

            var getProductQuery = new GetProductQuery
            {
                
            };

            var queryString = getProductQuery.ToQueryString();

            // Act
            var response = await _client.GetAsync($"{UrlHelper.ProductPath}{getProductQuery.ToQueryString()}");

            // Assert
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Result<IReadOnlyList<GetProductDto>>>();

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCountGreaterThan(0);
        }

        // Optional: Test invalid cases (e.g., unauthorized access, bad data)
        [Fact]
        public async Task CreateProduct_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var createProductCommand = new CreateProductCommand
            {
                Name = "Unauthorized Product",
                Price = 99.99m
            };

            // Act
            var response = await _client.PostAsJsonAsync(UrlHelper.ProductPath, createProductCommand);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

}
