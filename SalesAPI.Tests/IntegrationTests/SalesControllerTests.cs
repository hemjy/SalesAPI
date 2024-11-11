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
using SalesAPI.Application.DTOs.Products;
using SalesAPI.Tests.Helpers;
using SalesAPI.Application.DTOs.SalesOrders;
using SaleAPI.Presentation.Controllers;

namespace SalesAPI.Tests.IntegrationTests
{
    public class SalesControllerTests(DatabaseFixture dbFixture) : BaseControllerTest<SalesController>(dbFixture)
    {

       
        [Fact]
        public async Task CreateSalesOrder_ShouldReturnsOkAtActionResult()
        {
            // Arrange
            await AuthenticateAsync(); 
            var createProductCommand = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m,
            };

            
            var productResponse = await _client.PostAsJsonAsync(UrlHelper.ProductPath, createProductCommand);
            var productResult = await productResponse.Content.ReadFromJsonAsync<Result<Guid>>();

            var createSalesOrderCommand = new CreateSalesOrderCommand
            {
                ProductId = productResult.Data, 
                Quantity = 1,
                CustomerId = Guid.NewGuid(),
                UnitPrice =  10
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/sales", createSalesOrderCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeEmpty();
        }

        // Step 3: Get all sales orders
        [Fact]
        public async Task GetAllSalesOrders_ReturnsOkResult_WithCorrectData()
        {
            // Arrange
            await AuthenticateAsync();  

            var getSalesOrdersQuery = new GetSalesOrdersQuery
            {
                // Add query parameters if necessary
            };

            // Act
            var response = await _client.GetAsync($"{UrlHelper.SalesPath}{getSalesOrdersQuery.ToQueryString()}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Result<IReadOnlyList<GetSalesOrderDto>>>();

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCountGreaterThan(0);
        }

        // Step 4: Delete a sales order
        [Fact]
        public async Task DeleteSalesOrder_ShouldReturnsOk()
        {
            // Arrange
            await AuthenticateAsync();

            var createProductCommand = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m,
            };


            var productResponse = await _client.PostAsJsonAsync(UrlHelper.ProductPath, createProductCommand);
            var productResult = await productResponse.Content.ReadFromJsonAsync<Result<Guid>>();



            // Create a test sales order first (or use an existing one)
            var createSalesOrderCommand = new CreateSalesOrderCommand
            {
                ProductId = productResult.Data,
                Quantity = 1,
                CustomerId = Guid.NewGuid(),
                UnitPrice = 10
            };

            var createResponse = await _client.PostAsJsonAsync(UrlHelper.SalesPath, createSalesOrderCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdSalesOrder = await createResponse.Content.ReadFromJsonAsync<Result<Guid>>();
            var salesOrderId = createdSalesOrder.Data;

            // Act
            var deleteResponse = await _client.DeleteAsync($"{UrlHelper.SalesPath}/{salesOrderId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await deleteResponse.Content.ReadFromJsonAsync<Result<Guid>>();

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(salesOrderId);
        }

        [Fact]
        public async Task CreateSalesOrder_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var createSalesOrderCommand = new CreateSalesOrderCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                CustomerId = Guid.NewGuid(),
                UnitPrice = 100.00m
            };

            // Act without authenticating (no token)
            var response = await _client.PostAsJsonAsync(UrlHelper.SalesPath, createSalesOrderCommand);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

}
