using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Xunit;
using InvTrack.API.Models;

namespace InventoryTrack.IntegrationTests.Products
{
    public class RealDbProductsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RealDbProductsTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/Products/GetAll");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            json.Should().Contain("SmartPhone"); // Assumes this product exists
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_IfExists()
        {
            var id = 5; // Assumes this ID exists
            var response = await _client.GetAsync($"/api/Products/GetById/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ShouldAddProduct()
        {
            var newProduct = new
            {
                Name = "Test Product",
                Description = "Test Desc",
                Price = 100,
                StockQuantity = 5,
                CategoryId = 1,
                SupplierId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/Products/Create", newProduct);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
