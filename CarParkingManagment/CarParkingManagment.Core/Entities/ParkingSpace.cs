namespace CarParkingManagment.Core.Entities;

public class ParkingSpace
{
    public int Id { get; set; }
    public bool IsOccupied { get; set; }

    public virtual ICollection<VehicleSession> Sessions { get; set; }
}
