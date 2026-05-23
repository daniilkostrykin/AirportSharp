using Microsoft.EntityFrameworkCore;
using RutAirport.database;
using RutAirport.dto;
using RutAirport.interfaces;
using RutAirport.model;

namespace RutAirport.api;

public static class FlightsEndpoints
{
    public static RouteGroupBuilder MapFlightsEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/flights").WithTags("Flights");

        group.MapGet("/", async (IFlightService flightService, IMapper mapper) =>
        {
            var flights = await flightService.GetAllAsync();
            return Results.Ok(flights.Select(mapper.Map));
        })
        .WithSummary("1. Получить список всех рейсов (Табло)");

        group.MapGet("/{id:guid}", async (Guid id, IFlightService flightService, IMapper mapper) =>
        {
            var flight = await flightService.GetByIdAsync(id);
            return flight != null 
                ? Results.Ok(mapper.Map(flight)) 
                : Results.NotFound(new { message = "Рейс не найден" });
        })
        .WithSummary("2. Получить детальную информацию о рейсе");

        
        group.MapGet("/{id:guid}/passengers", async (Guid id, AirportDbContext db, IMapper mapper) =>
        {
            var tickets = await db.Tickets
                .Include(t => t.Passenger) 
                .Where(t => t.FlightId == id)
                .AsNoTracking()
                .ToListAsync();

            var response = tickets.Select(mapper.MapToFlightPassenger).ToList();
            return Results.Ok(response);
        })
        .WithSummary("3. Получить список пассажиров, купивших билеты на этот рейс");

        return api;
    }
}