namespace AirportApp.model;

/// <summary>
/// Посадочный талон. Является связующим звеном между пассажиром и рейсом.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор посадочного талона.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор рейса, на который зарегистрирован пассажир.
    /// </summary>
    public Guid FlightId { get; set; }

    /// <summary>
    /// Идентификатор пассажира, которому принадлежит талон.
    /// </summary>
    public Guid PassengerId { get; set; }

    /// <summary>
    /// Навигационное свойство к пассажиру, купившему билет.
    /// </summary>
    public Passenger? Passenger { get; set; }
    
    /// <summary>
    /// Фактически занимаемое место в салоне.
    /// </summary>
    public string? SeatNumber { get; set; } = string.Empty; 

    /// <summary>
    /// Время прохождения регистрации и выдачи талона в формате UTC.
    /// </summary>
    public DateTime? CheckInTimeUtc { get; set; }

    /// <summary>
    /// Фактическая сумма, которую заплатил пассажир с учетом скидок.
    /// </summary>
    public decimal PaidPrice { get; set; }

    /// <summary>
    /// Класс обслуживания, выбранный при покупке билета.
    /// </summary>
    public ServiceClass BookingClass { get; set; } = ServiceClass.Economy;
}
