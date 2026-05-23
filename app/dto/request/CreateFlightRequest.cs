namespace RutAirport.dto.request;

public record CreateFlightRequest(
    string FlightNumber, 
    Guid OriginAirportId,
    Guid DestinationAirportId, 
    DateTime DepartureTimeUtc, 
    decimal BasePrice,
    string[] AllSeats 
);