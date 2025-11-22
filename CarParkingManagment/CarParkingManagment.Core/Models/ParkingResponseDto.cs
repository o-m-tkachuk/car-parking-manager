namespace CarParkingManagment.Core.Models;

public class ParkingResponseDto
{
    public string VehicleReg { get; set; } = string.Empty;
    public int SpaceNumber { get; set; }
    public DateTime TimeIn { get; set; }
}
