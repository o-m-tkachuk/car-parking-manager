using CarParkingManagment.Core.Entities;

namespace CarParkingManagment.Core.Data;

public class SeedData
{
    public static void Initialize(AppDbContext context, int totalSpaces = 50)
    {
        if (context.ParkingSpaces.Any())
            return;

        for (int i = 1; i <= totalSpaces; i++)
        {
            context.ParkingSpaces.Add(new ParkingSpace
            {
                Id = i,
                IsOccupied = false
            });
        }

        context.SaveChanges();
    }
}
