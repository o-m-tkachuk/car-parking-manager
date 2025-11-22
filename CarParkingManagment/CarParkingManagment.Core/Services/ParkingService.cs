using CarParkingManagment.Core.Data;
using CarParkingManagment.Core.Models;
using CarParkingManagment.Core.Entities;
using CarParkingManagment.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CarParkingManagment.Core.Services;

public class ParkingService : IParkingService
{
    private readonly AppDbContext _context;
    private readonly IPricingService _pricingService;

    public ParkingService(AppDbContext context,
        IPricingService pricingService)
    {
        _context = context;
        _pricingService = pricingService;
    }
    public async Task<ParkingExitDto> ExitVehicleAsync(string vehicleReg)
    {
        var session = await _context.VehicleSessions
            .FirstOrDefaultAsync(s => s.VehicleReg == vehicleReg && s.EndTime == null);

        if (session == null)
            throw new NotFoundException($"A session for Vehicle {vehicleReg} is not found!");

        session.EndTime = DateTime.UtcNow;
        session.TotalCharge = _pricingService.CalculateCharge((VehicleType)session.VehicleType, session.EndTime.Value - session.StartTime);

        var space = await _context.ParkingSpaces
            .FirstOrDefaultAsync(s => s.Id == session.SpaceId);
        if (space == null)
            throw new NotFoundException($"Parking space with ID {session.SpaceId} not found!");

        space.IsOccupied = false;

        await _context.SaveChangesAsync();

        return new ParkingExitDto
        {
            VehicleReg = vehicleReg,
            TimeIn = session.StartTime,
            TimeOut = session.EndTime.Value,
            VehicleCharge = (double)session.TotalCharge.Value
        };
    }

    public async Task<ParkingAvailabilityDto> GetAvailabilityAsync()
    {
        var spaces = await _context.ParkingSpaces
            .AsNoTracking()
            .GroupBy(_ => true)
            .Select(g => new {
                Total = g.Count(),
                Occupied = g.Count(x => x.IsOccupied)
            })
            .FirstOrDefaultAsync();

        var total = spaces?.Total ?? 0;
        var occupied = spaces?.Occupied ?? 0;

        return new ParkingAvailabilityDto
        {
            AvailableSpaces = total - occupied,
            OccupiedSpaces = occupied
        };
    }

    public async Task<ParkingResponseDto> ParkVehicleAsync(string vehicleReg, VehicleType vehicleType)
    {
        var firstFreeSpace = await _context.ParkingSpaces
            .FirstOrDefaultAsync(s => !s.IsOccupied);

        if (firstFreeSpace == null) 
            throw new NotFoundException("No available spaces found!");

        var newSession = new VehicleSession
        {
            VehicleType = (int)vehicleType,
            VehicleReg = vehicleReg,
            SpaceId = firstFreeSpace.Id,
            StartTime = DateTime.UtcNow
        };
        _context.VehicleSessions.Add(newSession);

        firstFreeSpace.IsOccupied = true;

        await _context.SaveChangesAsync();

        return new ParkingResponseDto
        {
            VehicleReg = vehicleReg,
            SpaceNumber = firstFreeSpace.Id,
            TimeIn = newSession.StartTime
        };
    }
}
