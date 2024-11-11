using SalesAPI.Presentation.Controllers;
using System.Net;
using SalesAPI.Application.Features.Auth.Commands;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.Auth;
using SalesAPI.Tests.Infrastructure;

namespace SalesAPI.Tests.IntegrationTests
{
    public class AuthControllerTests(DatabaseFixture dbFixture) :   BaseControllerTest<AuthController>(dbFixture)
    {
        [Theory]
        [InlineData("test@test.com", "secret@12345")]
        [InlineData("admin@test.com", "Admin@12345")]
        [InlineData("admin@admin.com", "admin@12345")]
        public async Task ShouldReturnUnauthorized_WhenInvalidCredential(string email, string password)
        {
            // Arrange
            var payload = new LoginCommand { Email = email, Password = password };
            var payloadJson = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client
                .PostAsync($"/api/v1/Auth/login", payloadJson);


            var result = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var responseData = JsonConvert.DeserializeObject<Result<TokenDto>>(result);
            responseData.Should().NotBeNull();
            responseData.Succeeded.Should().BeFalse();
            responseData.Data.Should().BeNull();

        }

        [Theory]
        [InlineData("admin@admin.com", "Admin@12345")]
        [InlineData("Admin@admin.com", "Admin@12345")]
        [InlineData("admin@admin.com ", "Admin@12345")]
        [InlineData(" admin@admin.com ", "Admin@12345")]
        [InlineData("Admin@admin.com ", "Admin@12345")]
        public async Task ShouldReturnToken_WhenCredentialIsValid(string email, string password)
        {
            // Arrange
            var payload = new LoginCommand { Email = email, Password = password };
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

        }
    }
}
