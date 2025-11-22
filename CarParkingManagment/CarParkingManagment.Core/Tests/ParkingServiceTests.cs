using CarParkingManagment.Core.Data;
using CarParkingManagment.Core.Entities;
using CarParkingManagment.Core.Exceptions;
using CarParkingManagment.Core.Models;
using CarParkingManagment.Core.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarParkingManagment.Core.Tests;

public class ParkingServiceTests
{
    private readonly Mock<IPricingService> _pricingServiceMock = new Mock<IPricingService>();
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task ParkVehicleAsync_ShouldParkVehicle_WhenSpaceAvailable()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.ParkingSpaces.Add(new ParkingSpace { Id = 1, IsOccupied = false });
        await context.SaveChangesAsync();

        var service = new ParkingService(context, _pricingServiceMock.Object);

        // Act
        var result = await service.ParkVehicleAsync("ABC123", VehicleType.Small);

        // Assert
        Assert.Equal("ABC123", result.VehicleReg);
        Assert.Equal(1, result.SpaceNumber);
        Assert.True(result.TimeIn <= DateTime.UtcNow);
        var space = await context.ParkingSpaces.FindAsync(1);
        Assert.True(space.IsOccupied);
    }

    [Fact]
    public async Task ParkVehicleAsync_ShouldThrowNotFound_WhenNoSpaceAvailable()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.ParkingSpaces.Add(new ParkingSpace { Id = 1, IsOccupied = true });
        await context.SaveChangesAsync();

        var service = new ParkingService(context, _pricingServiceMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.ParkVehicleAsync("ABC123", VehicleType.Small));
    }

    [Fact]
    public async Task ExitVehicleAsync_ShouldCalculateChargeAndFreeSpace()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.ParkingSpaces.Add(new ParkingSpace { Id = 1, IsOccupied = true });
        context.VehicleSessions.Add(new VehicleSession
        {
            VehicleReg = "ABC123",
            VehicleType = (int)VehicleType.Small,
            SpaceId = 1,
            StartTime = DateTime.UtcNow.AddMinutes(-10)
        });
        await context.SaveChangesAsync();

        _pricingServiceMock
            .Setup(p => p.CalculateCharge(VehicleType.Small, It.IsAny<TimeSpan>()))
            .Returns(5.0m);

        var service = new ParkingService(context, _pricingServiceMock.Object);

        // Act
        var result = await service.ExitVehicleAsync("ABC123");

        // Assert
        Assert.Equal("ABC123", result.VehicleReg);
        Assert.Equal(5.0, result.VehicleCharge);
        var session = await context.VehicleSessions.FirstAsync();
        Assert.NotNull(session.EndTime);
        var space = await context.ParkingSpaces.FindAsync(1);
        Assert.False(space.IsOccupied);
    }

    [Fact]
    public async Task ExitVehicleAsync_ShouldThrowNotFound_WhenSessionNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ParkingService(context, _pricingServiceMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.ExitVehicleAsync("XYZ123"));
    }

    [Fact]
    public async Task GetAvailabilityAsync_ShouldReturnCorrectCounts()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.ParkingSpaces.Add(new ParkingSpace { Id = 1, IsOccupied = true });
        context.ParkingSpaces.Add(new ParkingSpace { Id = 2, IsOccupied = false });
        await context.SaveChangesAsync();

        var service = new ParkingService(context, _pricingServiceMock.Object);

        // Act
        var availability = await service.GetAvailabilityAsync();

        // Assert
        Assert.Equal(1, availability.OccupiedSpaces);
        Assert.Equal(1, availability.AvailableSpaces);
    }
}
