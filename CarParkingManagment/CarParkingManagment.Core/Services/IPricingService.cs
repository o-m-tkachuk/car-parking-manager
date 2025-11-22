using CarParkingManagment.Core.Models;

namespace CarParkingManagment.Core.Services;

public interface IPricingService
{
    decimal CalculateCharge(VehicleType vehicleType, TimeSpan duration);
}
