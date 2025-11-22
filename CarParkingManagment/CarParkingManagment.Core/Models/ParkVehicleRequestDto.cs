using System.ComponentModel.DataAnnotations;

namespace CarParkingManagment.Core.Models;

public class ParkVehicleRequestDto
{
    public string VehicleReg { get; set; } = string.Empty;
    [EnumDataType(typeof(VehicleType), ErrorMessage = "Invalid vehicle type.")]
    public VehicleType VehicleType { get; set; }
}