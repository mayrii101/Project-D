using Microsoft.EntityFrameworkCore;
using Xunit;
using AzureSqlConnectionDemo.Models;
using AzureSqlConnectionDemo.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTests.Services
{
    public class TestCustomerService : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerService _service;

        public TestCustomerService()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new CustomerService(_context);

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            _context.Customers.AddRange(
                new Customer { Id = 1, BedrijfsNaam = "Company A", IsDeleted = false },
                new Customer { Id = 2, BedrijfsNaam = "Company B", IsDeleted = true }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsOnlyActiveCustomers()
        {
            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            Assert.Single(result); // Alleen 1 niet-verwijderde klant
            Assert.Equal("Company A", result[0].BedrijfsNaam);
        }

        [Theory]
        [InlineData(1, true)]  // Bestaande actieve klant
        [InlineData(2, false)] // Verwijderde klant
        [InlineData(99, false)] // Niet-bestaande klant
        public async Task GetCustomerByIdAsync_ReturnsCorrectCustomer(int id, bool shouldExist)
        {
            // Act
            var result = await _service.GetCustomerByIdAsync(id);

            // Assert
            if (shouldExist)
            {
                Assert.NotNull(result);
                Assert.Equal(id, result.Id);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task CreateCustomerAsync_AddsNewCustomer()
        {
            // Arrange
            var newCustomer = new Customer
            {
                BedrijfsNaam = "New Company",
                Email = "new@test.com"
            };

            // Act
            var result = await _service.CreateCustomerAsync(newCustomer);

            // Assert
            Assert.NotNull(result.Id);
            var dbCustomer = await _context.Customers.FindAsync(result.Id);
            Assert.Equal("New Company", dbCustomer.BedrijfsNaam);
        }

        [Fact]
        public async Task UpdateCustomerAsync_UpdatesExistingCustomer()
        {
            // Arrange
            var updateData = new Customer
            {
                BedrijfsNaam = "Updated Name",
                Email = "updated@test.com"
            };

            // Act
            var result = await _service.UpdateCustomerAsync(1, updateData);

            // Assert
            Assert.Equal("Updated Name", result.BedrijfsNaam);
            var dbCustomer = await _context.Customers.FindAsync(1);
            Assert.Equal("updated@test.com", dbCustomer.Email);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsNullForInvalidId()
        {
            // Arrange
            var updateData = new Customer { BedrijfsNaam = "Test" };

            // Act
            var result = await _service.UpdateCustomerAsync(99, updateData);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1, true)]   // Succesvolle soft delete
        [InlineData(2, false)]  // Al verwijderde klant
        [InlineData(99, false)] // Niet-bestaande klant
        public async Task SoftDeleteCustomerAsync_WorksCorrectly(int id, bool expectedResult)
        {
            // Act
            var result = await _service.SoftDeleteCustomerAsync(id);
            var dbCustomer = await _context.Customers.FindAsync(id);

            // Assert
            Assert.Equal(expectedResult, result);
            if (dbCustomer != null)
            {
                Assert.Equal(expectedResult, dbCustomer.IsDeleted);
            }
        }
    }
}