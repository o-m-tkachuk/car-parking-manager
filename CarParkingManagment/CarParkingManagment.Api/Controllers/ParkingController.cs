using Microsoft.AspNetCore.Mvc;
using CarParkingManagment.Core.Services;
using CarParkingManagment.Core.Models;

namespace CarParkingManagment.Api.Controllers;

[Route("parking")]
[ApiController]
public class ParkingController : ControllerBase
{
    private readonly IParkingService _parkingService;

    public ParkingController(IParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    [HttpPost]
    public async Task<ActionResult<ParkingResponseDto>> ParkVehicle([FromBody] ParkVehicleRequestDto request)
    {
        var result = await _parkingService.ParkVehicleAsync(request.VehicleReg, request.VehicleType);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ParkingAvailabilityDto>> GetAvailability()
    {
        var result = await _parkingService.GetAvailabilityAsync();
        return Ok(result);
    }

    [HttpPost("exit")]
    public async Task<ActionResult<ParkingExitDto>> ExitVehicle([FromBody] ExitVehicleRequestDto request)
    {
        var result = await _parkingService.ExitVehicleAsync(request.VehicleReg);
        return Ok(result);
    }
}
