namespace RutAirport.dto.request;

/// <summary>
/// DTO создания нового рейса. Идентификатор не передается, так как генерируется БД.
/// </summary>
/// <param name="FlightNumber">Номер рейса (например, "SU-101").</param>
/// <param name="Destination">Пункт назначения.</param>
/// <param name="DepartureTimeUtc">Время вылета в формате UTC.</param>
/// <param name="AllSeats">Массив всех конкретных мест, доступных в самолете.</param>
public record CreateFlightRequest(
    string FlightNumber, 
    string Destination, 
    DateTime DepartureTimeUtc, 
    string[] AllSeats 
);