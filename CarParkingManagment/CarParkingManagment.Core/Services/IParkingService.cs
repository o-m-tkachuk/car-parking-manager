using CarParkingManagment.Core.Models;

namespace CarParkingManagment.Core.Services;

public interface IParkingService
{
    Task<ParkingResponseDto> ParkVehicleAsync(string vehicleReg, VehicleType vehicleType);
    Task<ParkingAvailabilityDto> GetAvailabilityAsync();
    Task<ParkingExitDto> ExitVehicleAsync(string vehicleReg);
}
