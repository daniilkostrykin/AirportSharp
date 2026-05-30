namespace AirportApp.dto.request;

/// <summary>
/// DTO создания нового рейса в системе аэропорта.
/// </summary>
/// <param name="FlightNumber">Номер рейса.</param>
/// <param name="OriginAirportId">Идентификатор аэропорта отправления.</param>
/// <param name="DestinationAirportId">Идентификатор аэропорта назначения.</param>
/// <param name="AircraftId">Идентификатор воздушного судна.</param>
/// <param name="DepartureTimeUtc">Время вылета в формате UTC.</param>
/// <param name="BasePrice">Базовая стоимость билета.</param>
public record CreateFlightRequest(
    string FlightNumber, 
    Guid OriginAirportId,      
    Guid DestinationAirportId, 
    Guid AircraftId,           
    DateTime DepartureTimeUtc, 
    decimal BasePrice
);
