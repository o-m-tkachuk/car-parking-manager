using CarParkingManagment.Core.Models;

namespace CarParkingManagment.Core.Services;

public class PricingService : IPricingService
{
    public static readonly Dictionary<VehicleType, decimal> RatesPerMinute = new()
    {
        { VehicleType.Small, 0.10m },
        { VehicleType.Medium, 0.20m },
        { VehicleType.Large, 0.40m }
    };

    public const decimal ExtraChargePer5Minutes = 1m;
    public decimal CalculateCharge(VehicleType vehicleType, TimeSpan duration)
    {
        var baseCharge = duration.Minutes * RatesPerMinute[vehicleType];
        var extra = (duration.Minutes / 5) * ExtraChargePer5Minutes;
        var total = baseCharge + extra;

        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }
}
