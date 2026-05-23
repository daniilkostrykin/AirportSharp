using RutAirport.dto.response;
using RutAirport.model;

namespace RutAirport.dto;

/// <summary>
/// Реализация маппинга доменных моделей аэропорта в DTO ответов API.
/// Берет сырые данные из базы и упаковывает их в чистые контракты.
/// </summary>
public class Mapper : IMapper
{
    public FlightResponse Map(Flight flight) => new(
        flight.Id, 
        flight.FlightNumber, 
        Origin: flight.OriginAirport != null ? $"{flight.OriginAirport.IataCode} ({flight.OriginAirport.City})" : "Unknown",
        Destination: flight.DestinationAirport != null ? $"{flight.DestinationAirport.IataCode} ({flight.DestinationAirport.City})" : "Unknown",
        Gate: flight.DepartureGate?.Name, 
        flight.DepartureTimeUtc, 
        flight.BasePrice, 
        flight.TotalSeats, 
        flight.AvailableSeats, 
        flight.Status.ToString()
    );
    public PassengerResponse Map(Passenger passenger) => new(
        passenger.Id, 
        passenger.FullName, 
        passenger.PassportNumber, 
        passenger.IsVip
    );

    public TicketResponse Map(Ticket ticket) => new(
        ticket.Id, 
        ticket.FlightId, 
        ticket.PassengerId, 
        ticket.SeatNumber, 
        ticket.CheckInTimeUtc
    );
}