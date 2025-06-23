using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecureFlight.Api.Models;
using SecureFlight.Api.Utils;
using SecureFlight.Core.Entities;
using SecureFlight.Core.Interfaces;
using SecureFlight.Core.Passengers.Commands.Create;
using SecureFlight.Core.Passengers.Commands.Delete;

namespace SecureFlight.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightsController(IService<Flight> flightService, IMapper mapper, IService<Passenger> passengerService)
    : SecureFlightBaseController(mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FlightDataTransferObject>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseActionResult))]
    public async Task<IActionResult> Get()
    {
        var flights = await flightService.GetAllAsync();
        return MapResultToDataTransferObject<IReadOnlyList<Flight>, IReadOnlyList<FlightDataTransferObject>>(flights);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePassengerCommand command)
    {
        if (command == null)
        {
            return BadRequest("Passsenger data must be provided");
        }

        if (string.IsNullOrEmpty(command.FirstName) || string.IsNullOrEmpty(command.LastName))
        {
            return BadRequest("Passsenger city and name must be provided.");
        }

        var flight = await flightService.FindbyIdAsync(command.Id);
        if (flight == null)
        {
            return NotFound($"flight with code {code} not found");
        }

        flight.Passengers.Add(new Passenger { FirstName = command.FirstName, LastName = command.LastName });

        var result = flightRepository.Update(flight);

        if (result == null)
        {
            return StatusCode.Status500InternalServerError);
        }
    }
    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] RemovePassengerCommand command)
    {
        if (command == null)
        {
            return BadRequest("Passsenger data must be provided");
        }
        var passenger = await passengerService.FindbyIdAsync(command.Id);
        if (passenger == null)
        {
            return NotFound($"passenger with code {command.Id} not found");
        }

    }