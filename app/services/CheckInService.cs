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
        if (passenger == null)
            throw new ArgumentException("Пассажир не найден.");

        
        string targetSeat = request.SeatNumber ?? string.Empty;

        
        if (string.IsNullOrWhiteSpace(targetSeat))
        {
            var takenSeats = await db.Tickets
                .Where(t => t.FlightId == flight.Id && t.SeatNumber != null)
                .Select(t => t.SeatNumber)
                .ToListAsync();

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

            
            var targetSeatClass = flight.Aircraft.GetSeatClass(targetSeat);
            if (targetSeatClass != ticket.BookingClass)
            {
                throw new ArgumentException($"Вы не можете выбрать это место. Ваш тариф: {ticket.BookingClass}, а место относится к классу: {targetSeatClass}");
            }

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
    public async Task<Ticket> BuyTicketAsync(BuyTicketRequest request)
    {
        
        var flight = await db.Flights
            .Include(f => f.Aircraft)
            .FirstOrDefaultAsync(f => f.Id == request.FlightId);
            
        if (flight == null) 
            throw new ArgumentException("Рейс не найден.");

        
        var passenger = await db.Passengers.FindAsync(request.PassengerId);
        if (passenger == null) 
            throw new ArgumentException("Пассажир не найден.");

        
        var multiplier = flight.Aircraft!.GetMultiplier(request.BookingClass);
        var finalPrice = flight.BasePrice * multiplier;

        
        if (passenger.IsVip)
        {
            finalPrice *= 0.9m;
        }

        
        if (request.PaymentAmount < finalPrice)
        {
            throw new ArgumentException($"Недостаточно средств. Стоимость билета {request.BookingClass}: {finalPrice} руб.");
        }

        
        if (flight.AvailableSeats <= 0)
        {
            throw new InvalidOperationException("Свободных мест на данном рейсе больше нет.");
        }

        
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            FlightId = flight.Id,
            PassengerId = passenger.Id,
            BookingClass = request.BookingClass, 
            PaidPrice = finalPrice,              
            SeatNumber = null                    
        };

        
        flight.AvailableSeats--;

        db.Tickets.Add(ticket);
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
} 

