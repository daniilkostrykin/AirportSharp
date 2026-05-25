using Microsoft.EntityFrameworkCore;
using AirportApp.database;
using AirportApp.dto.request;
using AirportApp.interfaces;
using AirportApp.model;

namespace AirportApp.services;

/// <summary>
/// Сервис для работы с пассажирами. Обеспечивает ведение базы клиентов.
/// </summary>
public class PassengerService(AirportDbContext db) : IPassengerService
{
    public async Task<IReadOnlyList<Passenger>> GetAllAsync()
        => await db.Passengers.AsNoTracking().OrderBy(x => x.FullName).ToListAsync();

    public async Task<Passenger> AddAsync(CreatePassengerRequest request)
    {
        var passenger = new Passenger
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            PassportNumber = request.PassportNumber,
            IsVip = request.IsVip
        };

        db.Passengers.Add(passenger);
        await db.SaveChangesAsync();
        return passenger;
    }
}

