using AirportApp.model;
namespace AirportApp.dto.request;

/// <summary>
/// DTO запроса на покупку билета на рейс.
/// </summary>
/// <param name="FlightId">Идентификатор рейса.</param>
/// <param name="PassengerId">Идентификатор пассажира.</param>
/// <param name="BookingClass">Класс обслуживания для бронирования.</param>
/// <param name="PaymentAmount">Сумма оплаты за билет.</param>
public record BuyTicketRequest(
    Guid FlightId,
    Guid PassengerId,
    ServiceClass BookingClass, 
    decimal PaymentAmount
);
