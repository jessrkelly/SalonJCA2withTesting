using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonJCA2.Controllers;
using SalonJCA2.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SalonJCA2.Tests.Controller_Tests
{
    public class BookingsControllerTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            //Seed the database with test info 
            if (!databaseContext.UserRoles.Any())
            {
                databaseContext.UserRoles.Add(new IdentityUserRole<string> { UserId = "test", RoleId = "test-role" });
            }

            //Save changes to DB anr return context 
            databaseContext.SaveChanges();
            return databaseContext;
        }

        [Fact]

        //Test method for checking if Index action returns a view result with a Booking model
        public async Task Index_ReturnsViewResult_WithBookingModel()
        {
            //Arrange - get context of DB 
            var dbContext = GetDatabaseContext();  //use in-memory database
            var controller = new BookingsController(dbContext);  //pass the DB contect directly

            //Act - Valid ID for testing - Assuming '1' is a valid ID
            var result = await controller.Index(1); 

            //Assert - Chec result, check model is correct and checj the model is not Null
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Bookings>(viewResult.Model);
            Assert.NotNull(model);
        }
    }
}
