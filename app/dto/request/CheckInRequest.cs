namespace RutAirport.dto.request;

/// <summary>
/// DTO запроса на регистрацию пассажира на рейс.
/// </summary>
/// <param name="FlightId">Идентификатор рейса.</param>
/// <param name="TicketId">Идентификатор билета.</param>
/// <param name="SeatNumber">Желаемое место. Если оставить null, сработает умное автоназначение.</param>
public record CheckInRequest(
    Guid TicketId,
    string? SeatNumber 
);