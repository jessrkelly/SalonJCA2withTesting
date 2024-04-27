using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SalonJCA2.Controllers;
using SalonJCA2.Models;

namespace SalonJCA2.Tests.Controller_Tests
{
    public class HomeControllerTest
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly ApplicationDbContext _dbContext;

        public HomeControllerTest()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();

            //Setup memory in database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(options);

            //Seed the database
            //Add Service, Types and Products to DB and then save the changes 
            _dbContext.services.Add(new Services { id = 1, Name = "Service1", Price = 100, path = "path1", Productid = 1, Typeid = 1 });
            _dbContext.types.Add(new Types { id = 1, TypeName = "Type1" });
            _dbContext.products.Add(new Products { id = 1, Name = "Product1" });
            _dbContext.SaveChanges();
        }

        [Fact]
        //My Test method for the Home Controller 
        public void Index_ReturnsAViewResult_WithABookingModel()
        {
            //Arrange - check if the test of index returns a view with booking model
            var controller = new HomeController(_mockLogger.Object, _dbContext);

            //Act - Cast result to  View result
            var result = controller.Index() as ViewResult;

            //Assert - Check if null, Type bookings, services and priducts in the View data
            Assert.NotNull(result);
            var model = Assert.IsType<Bookings>(result.Model); 
            Assert.NotNull(result.ViewData["products"]); 
            Assert.Equal(1, ((System.Collections.Generic.List<SalonJCA2.Models.Services>)result.ViewData["services"]).Count);
        }
    }
}