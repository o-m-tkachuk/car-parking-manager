using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarParkingManagment.Core.Entities.Configurations;

public class VehicleSessionConfiguration : IEntityTypeConfiguration<VehicleSession>
{
    public void Configure(EntityTypeBuilder<VehicleSession> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.VehicleType).IsRequired();

        builder.HasOne(e => e.Space)
              .WithMany(s => s.Sessions)
              .HasForeignKey(e => e.SpaceId);
    }
}
