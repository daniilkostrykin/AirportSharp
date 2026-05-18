namespace RutAirport.dto.request;

/// <summary>
/// DTO запроса на регистрацию пассажира на рейс.
/// </summary>
/// <param name="FlightId">Идентификатор рейса.</param>
/// <param name="PassengerId">Идентификатор пассажира.</param>
/// <param name="SeatNumber">Желаемое место. Если оставить null, сработает умное автоназначение.</param>
public record CheckInRequest(
    Guid FlightId, 
    Guid PassengerId,
    string? SeatNumber 
);