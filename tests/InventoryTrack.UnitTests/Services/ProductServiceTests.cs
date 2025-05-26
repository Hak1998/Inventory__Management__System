using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyApp.Application.DTOs;
using MyApp.Application.Services;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;

namespace InventoryTrack.UnitTests.Services
{
    public class ProductServiceTests
    {
        // In your ProductServiceTests.cs
        private readonly Mock<IProductRepository> _mockRepo = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _service = new ProductService(
                _mockRepo.Object,
                new HttpContextAccessor(),
                _mockMapper.Object,
                _mockUow.Object
            );
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.True(result.Status.Success);
            Assert.Null(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var testProduct = new Product { Id = 1, Name = "Test Product", StockQuantity = 1,Price = 12.56m,CategoryId = 1, SupplierId = 1 };
            _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(testProduct); // Setup for ID=1

            // Act
            var response = await _service.GetProductByIdAsync(1); // Call with ID=1

            // Assert
            response.Should().NotBeNull();
            response.Result.Name.Should().Be("Test Product");
            _mockRepo.Verify(x => x.GetByIdAsync(1), Times.Once); // Verify ID=1
        }

        [Fact]
        public async Task CreateProductAsync_IgnoresDtoNavigationProperties()
        {
            // Arrange
            var testDto = CreateTestProductDto();
            testDto.CategoryName = "Should Be Ignored";
            testDto.SupplierName = "Should Be Ignored";

            _mockRepo.Setup(x => x.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = 1);

            // Act
            var result = await _service.CreateProductAsync(testDto);

            // Assert
            _mockRepo.Verify(x => x.AddAsync(It.Is<Product>(p =>
                p.Category == null && p.Supplier == null)), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesModifyFields()
        {
            // Arrange
            var existingProduct = CreateTestProduct();
            var updateDto = CreateTestProductDto();
            updateDto.ModifyUser = "testuser";

            _mockRepo.Setup(x => x.GetProductByIdForUpdAsync(1))
                .ReturnsAsync(existingProduct);

            // Act
            var result = await _service.UpdateProductAsync(updateDto);

            // Assert
            existingProduct.ModifyUser.Should().Be("testuser");
            existingProduct.ModifyDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task UpdateProductAsync_HandlesConcurrentModification()
        {
            // Arrange
            var existingProduct = CreateTestProduct();
            var dto1 = CreateTestProductDto();
            var dto2 = CreateTestProductDto();
            dto2.Name = "Updated Name";

            _mockRepo.SetupSequence(x => x.GetProductByIdForUpdAsync(1))
                .ReturnsAsync(existingProduct) // First call
                .ReturnsAsync(existingProduct); // Second call

            // Act
            var task1 = _service.UpdateProductAsync(dto1);
            var task2 = _service.UpdateProductAsync(dto2);
            await Task.WhenAll(task1, task2);

            // Assert
            task1.Result.Status.Success.Should().BeTrue();
            task2.Result.Status.Success.Should().BeTrue();
            existingProduct.Name.Should().Be("Updated Name"); // Last write wins
        }


        #region Private

        private Product CreateTestProduct(int id = 1)
        {
            return new Product
            {
                Id = id,
                Name = "Test Product",
                Description = "Description",
                Price = 9.99m,
                StockQuantity = 10,
                CategoryId = 1,
                SupplierId = 1,
                Category = new Category { Id = 1, Name = "Electronics" },
                Supplier = new Supplier { Id = 1, Name = "Tech Supplier" },
                CreateDate = DateTime.UtcNow.AddDays(-1),
                CreateUser = "system"
            };
        }

        private ProductDto CreateTestProductDto(int id = 1)
        {
            return new ProductDto
            {
                Id = id,
                Name = "Test Product",
                Description = "Description",
                Price = 9.99m,
                StockQuantity = 10,
                CategoryId = 1,
                SupplierId = 1,
                CategoryName = "Electronics",
                SupplierName = "Tech Supplier"
            };
        }
        #endregion
    }
}
