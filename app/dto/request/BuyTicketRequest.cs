/// <param name="PaymentAmount">Сумма, которую пассажир вносит для оплаты билета.</param>
public record BuyTicketRequest(Guid FlightId, Guid PassengerId, decimal PaymentAmount);