namespace CarParkingManagment.Core.Models;

public class ParkingExitDto
{
    public string VehicleReg { get; set; } = string.Empty;
    public double VehicleCharge { get; set; }
    public DateTime TimeIn { get; set; }
    public DateTime TimeOut { get; set; }
}
