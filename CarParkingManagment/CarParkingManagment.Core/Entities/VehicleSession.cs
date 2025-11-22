namespace CarParkingManagment.Core.Entities;

public class VehicleSession
{
    public int Id { get; set; }
    public int VehicleType { get; set; } 
    public string VehicleReg { get; set; }
    public int SpaceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? TotalCharge { get; set; }

    public virtual ParkingSpace Space { get; set; }
}
