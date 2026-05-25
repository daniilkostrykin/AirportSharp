namespace AirportApp.dto.request;

/// <summary>
/// DTO регистрации нового пассажира в системе аэропорта.
/// </summary>
/// <param name="FullName">Полное имя пассажира.</param>
/// <param name="PassportNumber">Серия и номер паспорта.</param>
/// <param name="IsVip">Признак VIP-статуса, дает привилегии при регистрации и овербукинге.</param>
public record CreatePassengerRequest(
    string FullName, 
    string PassportNumber, 
    bool IsVip
);
