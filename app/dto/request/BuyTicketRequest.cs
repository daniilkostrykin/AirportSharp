using RutAirport.model;
namespace RutAirport.dto.request;

public record BuyTicketRequest(
    Guid FlightId,
    Guid PassengerId,
    ServiceClass BookingClass, 
    decimal PaymentAmount
);