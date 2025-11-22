using CarParkingManagment.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarParkingManagment.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    public DbSet<VehicleSession> VehicleSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
