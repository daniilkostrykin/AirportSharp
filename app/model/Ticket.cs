namespace RutAirport.model;

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
    /// Фактически занимаемое место в салоне.
    /// </summary>
    public string SeatNumber { get; set; } = string.Empty; 

    /// <summary>
    /// Время прохождения регистрации и выдачи талона в формате UTC.
    /// </summary>
    public DateTime CheckInTimeUtc { get; set; }
}