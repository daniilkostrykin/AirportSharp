using RutAirport.dto.request;
using RutAirport.model;

namespace RutAirport.interfaces;

/// <summary>
/// Интерфейс сервиса регистрации пассажиров и выдачи посадочных талонов.
/// </summary>
public interface ICheckInService
{
    /// <summary>
    /// Зарегистрировать пассажира на рейс с ручным выбором места или автоназначением.
    /// </summary>
    Task<Ticket> RegisterPassengerAsync(CheckInRequest request);

    /// <summary>
    /// Отменить регистрацию на рейс и освободить место.
    /// </summary>
    Task<bool> CancelCheckInAsync(Guid ticketId);
}