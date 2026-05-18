namespace RutAirport.dto.response;

/// <summary>
/// DTO стандартной ошибки API. Возвращается при неудачных проверках бизнес-логики.
/// </summary>
/// <param name="Message">Текстовое описание проблемы.</param>
public record ErrorResponse(string Message);