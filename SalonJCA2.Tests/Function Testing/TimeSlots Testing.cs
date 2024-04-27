using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SalonJCA2.Controllers;
using SalonJCA2.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SalonJCA2.Tests.Function_Testing
{ 
public class BookingTests
{
    // Declare _mockContext and _controller as fields accessible within the class
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly BookingsController _controller;

    public BookingTests()
    {
        // Initialize _mockContext
        _mockContext = new Mock<ApplicationDbContext>();

        // Mock DbSet for Times
        var timesData = new List<SalonJCA2.Models.Times>
        {
            new SalonJCA2.Models.Times { Id = 1, timeRang = "08:00 - 09:00" },
            new SalonJCA2.Models.Times { Id = 2, timeRang = "09:00 - 10:00" },
        }.AsQueryable();

        var mockTimes = new Mock<DbSet<SalonJCA2.Models.Times>>();
        mockTimes.As<IQueryable<SalonJCA2.Models.Times>>().Setup(m => m.Provider).Returns(timesData.Provider);
        mockTimes.As<IQueryable<SalonJCA2.Models.Times>>().Setup(m => m.Expression).Returns(timesData.Expression);
        mockTimes.As<IQueryable<SalonJCA2.Models.Times>>().Setup(m => m.ElementType).Returns(timesData.ElementType);
        mockTimes.As<IQueryable<SalonJCA2.Models.Times>>().Setup(m => m.GetEnumerator()).Returns(timesData.GetEnumerator());

        _mockContext.Setup(m => m.times).Returns(mockTimes.Object);

        // Initialize _controller with the mocked context
        _controller = new BookingsController(_mockContext.Object);
    }

    [Fact]
    public async Task CanBookWhenSlotIsAvailable()
    {
        // Act
        var result = await _controller.Add(new Bookings { Time = "10:00 - 11:00", Date = System.DateTime.Now.AddDays(1) });

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task CannotBookWhenSlotIsTaken()
    {
        // Act
        var result = await _controller.Add(new Bookings { Time = "08:00 - 09:00", Date = System.DateTime.Now });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
   }
}