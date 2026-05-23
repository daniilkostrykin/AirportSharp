namespace RutAirport.dto.response;

/// <summary>
/// DTO ответа с данными рейса (используется для вывода на табло аэропорта).
/// </summary>
/// <param name="Id">Уникальный идентификатор рейса.</param>
/// <param name="FlightNumber">Номер рейса.</param>
/// <param name="Destination">Город назначения.</param>
/// <param name="DepartureTimeUtc">Время вылета.</param>
/// <param name="TotalSeats">Общая вместимость салона.</param>
/// <param name="AvailableSeats">Количество оставшихся свободных мест.</param>
/// <param name="Status">Текущий статус рейса в виде строки (например, "Scheduled").</param>
public record FlightResponse(
    Guid Id, 
    string FlightNumber, 
    string Origin,       
    string Destination,  
    string? Gate,     
    DateTime DepartureTimeUtc, 
    decimal BasePrice, 
    int TotalSeats, 
    int AvailableSeats, 
    string Status
);