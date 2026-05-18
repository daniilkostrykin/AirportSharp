using Microsoft.EntityFrameworkCore;
using RutAirport.database;
using RutAirport.dto.request;
using RutAirport.interfaces;
using RutAirport.model;

namespace RutAirport.services;

/// <summary>
/// Сервис регистрации пассажиров. Реализует правила распределения мест и VIP-овербукинга.
/// </summary>
public class CheckInService(AirportDbContext db) : ICheckInService
{
    public async Task<Ticket> RegisterPassengerAsync(CheckInRequest request)
    {
        var flight = await db.Flights.FindAsync(request.FlightId);
        if (flight == null) throw new ArgumentException("Рейс не найден.");

        if (flight.Status == FlightStatus.Departed || flight.Status == FlightStatus.Cancelled)
            throw new InvalidOperationException("Регистрация на этот рейс закрыта.");

        var passenger = await db.Passengers.FindAsync(request.PassengerId);
        if (passenger == null) throw new ArgumentException("Пассажир не найден.");

        bool alreadyCheckedIn = await db.Tickets.AnyAsync(t => t.FlightId == flight.Id && t.PassengerId == passenger.Id);
        if (alreadyCheckedIn)
            throw new InvalidOperationException("Пассажир уже зарегистрирован на этот рейс.");

        string targetSeat = request.SeatNumber ?? string.Empty;

        if (string.IsNullOrWhiteSpace(targetSeat))
        {
            var takenSeats = await db.Tickets
                .Where(t => t.FlightId == flight.Id)
                .Select(t => t.SeatNumber)
                .ToListAsync();

            targetSeat = flight.AllSeats.FirstOrDefault(s => !takenSeats.Contains(s));

            if (targetSeat == null)
            {
                if (!passenger.IsVip)
                    throw new InvalidOperationException("Нет свободных мест для автоназначения. Доступен только VIP-овербукинг.");
                
                targetSeat = $"VIP-{(new Random().Next(100, 999))}";
            }
            else
            {
                flight.AvailableSeats--; 
            }
        }
        else
        {
            bool isSeatTaken = await db.Tickets.AnyAsync(t => t.FlightId == flight.Id && t.SeatNumber == targetSeat);
            if (isSeatTaken)
                throw new InvalidOperationException($"Место {targetSeat} уже занято.");

            if (!flight.AllSeats.Contains(targetSeat))
            {
                if (!passenger.IsVip)
                    throw new ArgumentException($"Места {targetSeat} не существует на этом рейсе.");
            }
            else
            {
                flight.AvailableSeats--;
            }
        }

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            FlightId = flight.Id,
            PassengerId = passenger.Id,
            SeatNumber = targetSeat,
            CheckInTimeUtc = DateTime.UtcNow
        };

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        return ticket;
    }

    public async Task<bool> CancelCheckInAsync(Guid ticketId)
    {
        var ticket = await db.Tickets.FindAsync(ticketId);
        if (ticket == null) return false;

        var flight = await db.Flights.FindAsync(ticket.FlightId);
        if (flight != null && flight.AllSeats.Contains(ticket.SeatNumber))
        {
            flight.AvailableSeats++; 
        }

        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        return true;
    }
}
