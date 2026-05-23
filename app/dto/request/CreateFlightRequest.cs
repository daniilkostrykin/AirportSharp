namespace RutAirport.dto.request;

public record CreateFlightRequest(
    string FlightNumber, 
    Guid OriginAirportId,      
    Guid DestinationAirportId, 
    Guid AircraftId,           
    DateTime DepartureTimeUtc, 
    decimal BasePrice
);