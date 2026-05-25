using AirportApp.dto.response;
using AirportApp.interfaces;
using AirportApp.model;

namespace AirportApp.dto;

public class Mapper : IMapper
{
    public FlightResponse Map(Flight flight) => new(
        Id: flight.Id,
        FlightNumber: flight.FlightNumber,
        Origin: $"{flight.OriginAirport?.IataCode} ({flight.OriginAirport?.City})",
        Destination: $"{flight.DestinationAirport?.IataCode} ({flight.DestinationAirport?.City})",
        Gate: flight.DepartureGate?.Name,
        AircraftModel: flight.Aircraft?.ModelName ?? "Unknown",
        DepartureTimeUtc: flight.DepartureTimeUtc,
        BasePrice: flight.BasePrice,
        TotalSeats: flight.Aircraft?.TotalSeats ?? 0,
        AvailableSeats: flight.AvailableSeats,
        Status: flight.Status.ToString() 
    );

    public PassengerResponse Map(Passenger passenger) => new(
        Id: passenger.Id,
        FullName: passenger.FullName,
        PassportNumber: passenger.PassportNumber,
        IsVip: passenger.IsVip
    );

    public TicketResponse Map(Ticket ticket) => new(
        ticket.Id,
        ticket.FlightId,
        ticket.PassengerId,
        ticket.SeatNumber,
        ticket.BookingClass,
        ticket.PaidPrice,
        ticket.CheckInTimeUtc
    ); 

    public object MapToFlightPassenger(Ticket ticket) => new
    {
        PassengerId = ticket.PassengerId,
        FullName = ticket.Passenger?.FullName ?? "Неизвестно", 
        PassportNumber = ticket.Passenger?.PassportNumber ?? "Неизвестно",
        IsVip = ticket.Passenger?.IsVip ?? false,
        SeatNumber = ticket.SeatNumber,
        BookingClass = ticket.BookingClass,
        PaidPrice = ticket.PaidPrice
    };
}
