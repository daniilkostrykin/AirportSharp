using Microsoft.EntityFrameworkCore;
using RutAirport.database;
using RutAirport.dto.request;
using RutAirport.interfaces;
using RutAirport.model;

namespace RutAirport.services;
/// <summary>
/// Сервис для управления рейсами. Отвечает за расписание, добавление бортов и изменение их статусов.
/// </summary>
public class FlightService(AirportDbContext db) : IFlightService
{
    public async Task<IReadOnlyList<Flight>> GetAllAsync()
        => await db.Flights
            .Include(f => f.OriginAirport)      
            .Include(f => f.DestinationAirport) 
            .Include(f => f.DepartureGate)     
            .AsNoTracking()
            .OrderBy(x => x.DepartureTimeUtc)
            .ToListAsync();

    public async Task<Flight> AddAsync(CreateFlightRequest request)
    {
        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = request.FlightNumber,
            OriginAirportId = request.OriginAirportId,
            DestinationAirportId = request.DestinationAirportId,
            DepartureTimeUtc = request.DepartureTimeUtc,
            BasePrice = request.BasePrice,                  
            AllSeats = request.AllSeats,
            TotalSeats = request.AllSeats.Length,
            AvailableSeats = request.AllSeats.Length,
            Status = FlightStatus.Scheduled,
            DepartureGateId = null 
        };

        db.Flights.Add(flight);
        await db.SaveChangesAsync();
        return flight;
    }

    public async Task<Flight?> ChangeStatusAsync(Guid id, FlightStatus newStatus)
    {
        var flight = await db.Flights.FindAsync(id);
        if (flight == null) return null;

        flight.Status = newStatus;
        await db.SaveChangesAsync();
        return flight;
    }

    public async Task<Flight?> GetByIdAsync(Guid id)
        => await db.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .Include(f => f.DepartureGate)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
}
