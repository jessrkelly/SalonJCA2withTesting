using System;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonJCA2.Controllers;
using SalonJCA2.Models;
using Microsoft.AspNetCore.Mvc;
namespace SalonJCA2.Tests.Controller_Tests
{
    public class BookingsControllerTests
    {
        // Use a method to create a new instance of DbContext with InMemory provider
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // Make sure to use a unique name for each test run
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            // Seed the database with test data
            if (!databaseContext.UserRoles.Any())
            {
                databaseContext.UserRoles.Add(new IdentityUserRole<string> { UserId = "test", RoleId = "test-role" });
                // ... add any other seeding required for the tests
            }

            databaseContext.SaveChanges();
            return databaseContext;
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithABookingModel()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new BookingsController(context);
            int testServiceId = 1; // You should have this ID in your seeded data

            // Act
            var result = controller.Index(testServiceId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Bookings>(viewResult.Model);
            Assert.NotNull(model); // Verify that a Bookings model is passed to the view
                                   // Further assertions to check the data in the model (if needed)
        }
    }
}