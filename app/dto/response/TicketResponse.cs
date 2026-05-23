namespace RutAirport.dto.response;

/// <summary>
/// DTO электронного посадочного талона, выданного после успешной регистрации.
/// </summary>
/// <param name="Id">Уникальный идентификатор посадочного талона.</param>
/// <param name="FlightId">Идентификатор рейса, на который зарегистрирован пассажир.</param>
/// <param name="PassengerId">Идентификатор пассажира, которому принадлежит талон.</param>
/// <param name="SeatNumber">Фактически назначенное место в салоне (реальное или виртуальное VIP).</param>
/// <param name="CheckInTimeUtc">Время прохождения регистрации в формате UTC.</param>
public record TicketResponse(
    Guid Id, 
    Guid FlightId, 
    Guid PassengerId, 
    string? SeatNumber, 
    DateTime? CheckInTimeUtc
);