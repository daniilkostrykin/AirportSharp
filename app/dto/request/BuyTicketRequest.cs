using AirportApp.model;
namespace AirportApp.dto.request;

public record BuyTicketRequest(
    Guid FlightId,
    Guid PassengerId,
    ServiceClass BookingClass, 
    decimal PaymentAmount
);
