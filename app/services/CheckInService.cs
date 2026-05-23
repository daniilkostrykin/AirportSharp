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
        var ticket = await db.Tickets.FindAsync(request.TicketId);
        if (ticket == null) throw new ArgumentException("Билет не найден.");
        if (ticket.SeatNumber != null) throw new InvalidOperationException("Вы уже прошли регистрацию.");

        var flight = await db.Flights
            .Include(f => f.Aircraft) 
            .FirstOrDefaultAsync(f => f.Id == ticket.FlightId); 

        if (flight == null) 
            throw new ArgumentException("Рейс не найден.");
            
        var passenger = await db.Passengers.FindAsync(ticket.PassengerId);
        
        string targetSeat = request.SeatNumber ?? string.Empty;

        var takenSeats = await db.Tickets
            .Where(t => t.FlightId == flight.Id && t.SeatNumber != null)
            .Select(t => t.SeatNumber)
            .ToListAsync();

        if (string.IsNullOrWhiteSpace(targetSeat))
        {
            targetSeat = flight.Aircraft!.SeatMap.FirstOrDefault(s => !takenSeats.Contains(s))!;
            if (targetSeat == null)
            {
                if (!passenger.IsVip)
                    throw new InvalidOperationException("Физических мест нет. Регистрация закрыта.");

                var victimTicket = await (from t in db.Tickets
                                          join p in db.Passengers on t.PassengerId equals p.Id
                                          where t.FlightId == flight.Id && !p.IsVip && t.SeatNumber != null
                                          orderby t.CheckInTimeUtc descending
                                          select t).FirstOrDefaultAsync();

                if (victimTicket != null)
                {
                    targetSeat = victimTicket.SeatNumber!;
                    victimTicket.SeatNumber = null;
                    victimTicket.CheckInTimeUtc = null;
                }
                else
                {
                    targetSeat = $"VIP-{(new Random().Next(100, 999))}";
                }
            }
        }
        else
        {
            if (!flight.Aircraft!.SeatMap.Contains(targetSeat))
                throw new ArgumentException("Такого места нет в самолете.");

            var existingTicket = await db.Tickets.FirstOrDefaultAsync(t => t.FlightId == flight.Id && t.SeatNumber == targetSeat);
            
            if (existingTicket != null)
            {
                if (!passenger.IsVip) throw new InvalidOperationException("Место занято.");

                var occupant = await db.Passengers.FindAsync(existingTicket.PassengerId);
                if (occupant != null && !occupant.IsVip)
                {
                    existingTicket.SeatNumber = null;
                    existingTicket.CheckInTimeUtc = null;
                }
                else
                {
                    throw new InvalidOperationException("Место занято другим VIP-клиентом.");
                }
            }
        }

        ticket.SeatNumber = targetSeat;
        ticket.CheckInTimeUtc = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> CancelCheckInAsync(Guid ticketId)
    {
        var ticket = await db.Tickets.FindAsync(ticketId);
        if (ticket == null || ticket.SeatNumber == null) return false;

        ticket.SeatNumber = null;
        ticket.CheckInTimeUtc = null;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<Ticket> BuyTicketAsync(BuyTicketRequest request)
    {
        var flight = await db.Flights.FindAsync(request.FlightId);
        if (flight == null) throw new ArgumentException("Рейс не найден.");

        var passenger = await db.Passengers.FindAsync(request.PassengerId);
        if (passenger == null) throw new ArgumentException("Пассажир не найден.");

        bool alreadyBought = await db.Tickets.AnyAsync(t => t.FlightId == flight.Id && t.PassengerId == passenger.Id);
        if (alreadyBought) throw new InvalidOperationException("Вы уже купили билет на этот рейс.");

        if (flight.AvailableSeats <= 0 && !passenger.IsVip)
            throw new InvalidOperationException("Все билеты распроданы.");

        decimal finalPrice = passenger.IsVip 
            ? flight.BasePrice * 0.9m 
            : flight.BasePrice;

        if (request.PaymentAmount < finalPrice)
            throw new InvalidOperationException($"Недостаточно средств. Стоимость билета: {finalPrice} руб.");


        if (flight.AvailableSeats > 0) 
            flight.AvailableSeats--;

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            FlightId = flight.Id,
            PassengerId = passenger.Id,
            SeatNumber = null, 
            CheckInTimeUtc = null,
            PaidPrice = finalPrice
        };

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        return ticket;
    }
}
