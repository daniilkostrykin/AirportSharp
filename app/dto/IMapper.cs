using AirportApp.dto.response;
using AirportApp.model;

namespace AirportApp.dto;

/// <summary>
/// Контракт маппера доменных моделей аэропорта в DTO ответов API.
/// </summary>
public interface IMapper
{
    /// <summary>
    /// Преобразует сущность рейса в безопасный DTO ответа API.
    /// </summary>
    FlightResponse Map(Flight flight);

    /// <summary>
    /// Преобразует сущность пассажира в DTO ответа API.
    /// </summary>
    PassengerResponse Map(Passenger passenger);

    /// <summary>
    /// Преобразует посадочный талон из базы в DTO ответа API.
    /// </summary>
    TicketResponse Map(Ticket ticket);

    /// <summary>
    /// Преобразует посадочный талон в безопасный DTO ответа для списка пассажиров рейса.
    /// </summary>
    object MapToFlightPassenger(Ticket ticket);
}
