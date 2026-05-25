namespace AirportApp.dto.response;

/// <summary>
/// DTO ответа с публичными данными пассажира.
/// </summary>
/// <param name="Id">Уникальный идентификатор пассажира в базе данных.</param>
/// <param name="FullName">Полное имя (ФИО) пассажира.</param>
/// <param name="PassportNumber">Серия и номер паспорта.</param>
/// <param name="IsVip">Признак наличия VIP-статуса у пассажира.</param>
public record PassengerResponse(
    Guid Id, 
    string FullName, 
    string PassportNumber, 
    bool IsVip
);
