using Microsoft.EntityFrameworkCore;
using AirportApp.database;
using AirportApp.dto.request;
using AirportApp.interfaces;
using AirportApp.model;

namespace AirportApp.services;

/// <summary>
/// Сервис для работы с рейсами. Обеспечивает просмотр, создание и изменение статусов рейсов.
/// </summary>
public class FlightService(AirportDbContext db) : IFlightService
{
    public async Task<IReadOnlyList<Flight>> GetAllAsync()
        => await db.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .Include(f => f.DepartureGate)
            .Include(f => f.Aircraft) 
            .AsNoTracking()
            .OrderBy(x => x.DepartureTimeUtc)
            .ToListAsync();

    public async Task<Flight?> GetByIdAsync(Guid id)
        => await db.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .Include(f => f.DepartureGate)
            .Include(f => f.Aircraft)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Flight> AddAsync(CreateFlightRequest request)
    {
        var aircraft = await db.Aircrafts.FindAsync(request.AircraftId);
        if (aircraft == null) throw new ArgumentException("Указанный самолет не найден в парке.");

        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = request.FlightNumber,
            OriginAirportId = request.OriginAirportId,
            DestinationAirportId = request.DestinationAirportId,
            AircraftId = request.AircraftId,
            DepartureTimeUtc = request.DepartureTimeUtc,
            BasePrice = request.BasePrice,                  
            AvailableSeats = aircraft.TotalSeats, 
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
}
