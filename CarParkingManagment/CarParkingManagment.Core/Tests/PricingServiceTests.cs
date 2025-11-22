using CarParkingManagment.Core.Models;
using CarParkingManagment.Core.Services;
using Xunit;

namespace CarParkingManagment.Core.Tests;

public class PricingServiceTests
{
    private readonly IPricingService _pricingService;

    public PricingServiceTests()
    {
        _pricingService = new PricingService();
    }

    [Theory]
    [InlineData(VehicleType.Small, 10.12, 3)] 
    [InlineData(VehicleType.Medium, 5.5, 2)]  
    [InlineData(VehicleType.Large, 7.24, 3.8)]  
    [InlineData(VehicleType.Small, 4.3, 0.4)]  
    [InlineData(VehicleType.Medium, 15.4, 6)]
    public void CalculateCharge_ReturnsExpectedValue(VehicleType vehicleType, double minutes, decimal expectedCharge)
    {
        // Arrange
        var duration = TimeSpan.FromMinutes(minutes);

        // Act
        var charge = _pricingService.CalculateCharge(vehicleType, duration);

        // Assert
        Assert.Equal(expectedCharge, charge);
    }

    [Fact]
    public void RatesPerMinute_ShouldContainAllVehicleTypes()
    {
        Assert.Contains(VehicleType.Small, PricingService.RatesPerMinute.Keys);
        Assert.Contains(VehicleType.Medium, PricingService.RatesPerMinute.Keys);
        Assert.Contains(VehicleType.Large, PricingService.RatesPerMinute.Keys);
    }
}