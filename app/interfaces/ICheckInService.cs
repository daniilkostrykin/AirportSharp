using AirportApp.dto.request;
using AirportApp.model;

namespace AirportApp.interfaces;

/// <summary>
/// Интерфейс сервиса регистрации пассажиров и выдачи посадочных талонов.
/// </summary>
public interface ICheckInService
{
    Task<Ticket> BuyTicketAsync(BuyTicketRequest request);
    /// <summary>
    /// Зарегистрировать пассажира на рейс с ручным выбором места или автоназначением.
    /// </summary>
    Task<Ticket> RegisterPassengerAsync(CheckInRequest request);

    /// <summary>
    /// Отменить регистрацию на рейс и освободить место.
    /// </summary>
    Task<bool> CancelCheckInAsync(Guid ticketId);
}
